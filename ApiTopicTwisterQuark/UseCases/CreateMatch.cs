using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases
{
    public class CreateMatch 
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        private CategoryAndLetterGenerator categoryAndLetterGenerator;
      
        List<string> categories; 
        List<char> letters;
        
        public CreateMatch(IMatchRepository matchRepository,IRoundRepository roundRepository,ITurnRepository turnRepository,IAnswerRepository answerRepository,CategoryAndLetterGenerator categoryAndLetterGenerator)
        {
            this.matchRepository = matchRepository;
            this.roundRepository = roundRepository;
            this.turnRepository = turnRepository;
            this.answerRepository = answerRepository;
            this.categoryAndLetterGenerator = categoryAndLetterGenerator;
        }
        
        public async Task<string> CreateMatchWithThisPlayersIDsAndNumberOfRounds(List<string> playersIDs, int numberOfRounds,int categoriesPerRound, float timePerTurn)
        {
            string matchID = await matchRepository.GenerateNewMatchID();
            Match match = new Match(matchID, playersIDs, "", false);

            List<Round> rounds = await CreateRounds(matchID, numberOfRounds, categoriesPerRound);

            foreach (var round in rounds)
            {

               List<int> turnIDs = await CreateTurns(matchID, round.ID, timePerTurn, playersIDs);
               foreach (var turnID in turnIDs)
               {
                   await CreateAnswers(matchID, round.ID, turnID, round.Categories,round.Letter);
               }
               playersIDs.Reverse();

            }

            await matchRepository.Add(match);
            
            return match.ID;
        }
        private async Task GenerateCategoriesAndLettersForNumberOfRounds(int numberOfRounds, int categoriesPerRound)
        {
            letters = await categoryAndLetterGenerator.GenerateThisNumberOfLetters(numberOfRounds);
            categories = await categoryAndLetterGenerator.GenerateThisNumberOfCategories(categoriesPerRound * numberOfRounds);
        }

        private async Task<List<Round>> CreateRounds(string matchID,int numberOfRounds,int categoriesPerRound)
        {
            await GenerateCategoriesAndLettersForNumberOfRounds(numberOfRounds,categoriesPerRound);
            List<Round> rounds = new List<Round>();
            for (int roundID = 1; roundID <= numberOfRounds; roundID++)
            {
                Round round = new Round(matchID,roundID, categories.Take(categoriesPerRound).ToList(), letters[0], "",
                    false, 60);
                categories.RemoveRange(0, categoriesPerRound);
                letters.RemoveAt(0);
                await roundRepository.Add(round);
                rounds.Add(round);
            }

            return rounds;

        }


        private async Task<List<int>> CreateTurns(string matchID,int roundID,float initialTime, List<string> playersIDs)
        {
            int index = 0;
            List<int> turnIDs = new List<int>();
            foreach (var playerID in playersIDs)
            {
                index++;
                Turn turn = new Turn(matchID,roundID,index,playerID, initialTime, false,0); 
                await turnRepository.Add(turn);
                turnIDs.Add(index);
            }
            
            return turnIDs;
        }

        private async Task CreateAnswers(string matchID,int roundID, int turnID,
            List<string> categoriesNames, char letter)
        {
            foreach (var categoryName in categoriesNames)
            {
                Answer answer = new Answer(matchID, roundID, turnID, categoryName, "", letter);
                await answerRepository.Add(answer);
            }
        }
        
        
    }
}