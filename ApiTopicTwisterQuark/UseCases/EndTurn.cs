using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class EndTurn
    {
        private ICategoryRepository categoriesRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        public EndTurn(ICategoryRepository categoriesRepository, ITurnRepository turnRepository,  IAnswerRepository answerRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.turnRepository = turnRepository;
            this.answerRepository = answerRepository;
        }
        
        public async Task SetAnswer(string matchID, int roundID, int turnID,string word, string categoryName)
        {
            Answer answer = await answerRepository.Get(matchID,roundID,turnID,categoryName);
            answer.word = (word!=null)?word: string.Empty;            
            await answerRepository.Update(answer);
        }
        
        public async Task EndThisTurn(DataApplication dataApplication, float timeLeft)
        {
            Turn turn = await turnRepository.Get(dataApplication.MatchID, dataApplication.RoundID, dataApplication.TurnID);
            turn.Time = timeLeft;
            await SetTurnAnswers(turn);
            turn.SetOver();
            await turnRepository.Update(turn);
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

        public async Task<TurnResultDTO> GetTurnResult(string matchID, int roundID, int turnID)
        {
            Turn turn = await turnRepository.Get(matchID, roundID, turnID);
            List<Answer> answers = await SetAnswersWithCategories(turn);
            return GenerateTurnResult(turn, answers);
        }

        private async Task<List<Answer>> SetAnswersWithCategories(Turn turn)
        {
            List<Answer> answers = await answerRepository.GetAnswersFromTurn(turn.MatchID, turn.RoundID, turn.ID);
            foreach (var answer in answers)
            {
                answer.SetCategory(await categoriesRepository.GetCategoryByName(answer.categoryName));
            };
            return answers;
        }

        private TurnResultDTO GenerateTurnResult(Turn turn,List<Answer> answers)
        {
            return new TurnResultDTO(answers.Select(a => AnswerToAnswerDTO(a)).ToList(), turn.Time);
        }


        private AnswerDTO AnswerToAnswerDTO(Answer answer)
        {
            return new AnswerDTO(answer.categoryName, answer.word,
                answer.Letter, answer.IsCorrect());
        }
        
    }
    
}