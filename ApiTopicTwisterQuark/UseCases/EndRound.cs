using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class EndRound
    {
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        ICategoryRepository categoriesRepository;

        public EndRound(IRoundRepository roundRepository, ITurnRepository turnRepository,
            IAnswerRepository answerRepository, ICategoryRepository categoriesRepository)
        {
            this.roundRepository = roundRepository;
            this.turnRepository = turnRepository;
            this.answerRepository = answerRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<bool> IsActualRoundOver(string matchID, int roundID)
        {
            Round round = await roundRepository.Get(matchID, roundID);
            round.Turns = await turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID);
            return round.AreTurnsOver();
        }

        public async Task EndThisRound(string matchID, int roundID)
        {
            Round round = await GetRoundWithTurnsAndAnswers(matchID, roundID);
            if (round.IsOver()) return;
            round.SetOver();
            await roundRepository.Update(round);
        }

        public async Task<RoundResultDTO> ReturnRoundResultForPlayer(string matchID, int roundID,string playerID)
        {
            Round round = await GetRoundWithTurnsAndAnswers(matchID, roundID);
            int numberOfRounds = await GetNumberOfRounds(matchID);
            return CreateRoundResult(round, numberOfRounds, playerID, round.GetRivalIDFromTurns(playerID));

        }

        private async Task<int> GetNumberOfRounds(string matchID)
        {
            List<Round> rounds = await roundRepository.GetRoundsFromMatchID(matchID);
            int numberOfRounds = rounds.Count;
            return numberOfRounds;
        }

        private async Task<Round> GetRoundWithTurnsAndAnswers(string matchID, int roundID)
        {
            Round round = await roundRepository.Get(matchID, roundID);
            List<Turn> turns = await turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID);
            foreach (var turn in turns)
            {
                await SetTurnAnswers(turn);
            }

            round.Turns = turns;
            return round;
        }

        private async Task SetTurnAnswers(Turn turn)
        {
            List<Answer> answers = await answerRepository.GetAnswersFromTurn(turn.MatchID, turn.RoundID, turn.ID);
            foreach (var answer in answers)
            {
                answer.SetCategory(await categoriesRepository.GetCategoryByName(answer.categoryName));
            }

            turn.SetAnswers(answers);
        }

        private RoundResultDTO CreateRoundResult(Round round, int numberOfRounds, string playerID, string rivalID)
        {
            Turn playerTurn = round.GetTurnOfPlayerID(playerID);
            Turn rivalTurn = round.GetTurnOfPlayerID(rivalID);
            RoundResultDTO roundResult = new RoundResultDTO(
                playerID, rivalID, round.ID, numberOfRounds, round.Letter,
                GenerateListAnswerComparers(round.Categories, playerTurn, rivalTurn),
                playerTurn.CorrectCount, playerTurn.Time,
                rivalTurn.CorrectCount, rivalTurn.Time, round.IsPlayerTheWinner(playerID), round.Tie
            );

            return roundResult;

        }
        
        private List<AnswerComparerDTO> GenerateListAnswerComparers(List<string> roundCategories, Turn playerTurn,
            Turn rivalTurn)
        {

            List<AnswerComparerDTO> answerComparers = roundCategories.Select(rc =>
                new AnswerComparerDTO( GenerateWordAndResult(playerTurn.GetAnswerOfCategoryName(rc)),
                    GenerateWordAndResult( rivalTurn.GetAnswerOfCategoryName(rc)),
                    rc)).ToList();
            return answerComparers;
        }


        private WordAndResultDTO GenerateWordAndResult(Answer answer)
        {
            return new WordAndResultDTO(answer.word, answer.IsCorrect());
        }


    }
}

    