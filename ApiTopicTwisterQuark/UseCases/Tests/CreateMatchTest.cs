using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
  
    public class CreateMatchTest
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        private CategoryAndLetterGenerator generator;
        private List<string> categories;
        private List<char> letters;
        private CreateMatch createMatch;

        [SetUp]
        public void Setup()
        {
            categories = new List<string>()
                { "Paises", "Ciudades", "Deportes", "Musica", "Arte", "Ciencia", "Tecnologia", "Naturaleza" };
            letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F' };
            matchRepository = Substitute.For<IMatchRepository>();
            roundRepository = Substitute.For<IRoundRepository>();
            turnRepository = Substitute.For<ITurnRepository>();
            answerRepository = Substitute.For<IAnswerRepository>();
            ICategoryRepository categoriesRepository = Substitute.For<ICategoryRepository>();
            ILettersRepository lettersRepository = Substitute.For<ILettersRepository>();
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            createMatch = new CreateMatch(matchRepository,roundRepository,turnRepository,answerRepository ,generator);
        }
        [Test] 
        public async Task CreateMatchWithThisPlayersIDsAndNumberOfRounds_NumberOfRoundsEqualsOne_ReturnMatchID()
        {
           
            var playersIDs = new List<string>() { "Player"};
            var numberOfRounds = 1;
            var categoriesPerRound = 1;
            var timePerTurn = 60;
            var matchID = "Match";

            matchRepository.GenerateNewMatchID().Returns(matchID);
            var result =  await createMatch.CreateMatchWithThisPlayersIDsAndNumberOfRounds(playersIDs, numberOfRounds, categoriesPerRound,
                timePerTurn);
            await matchRepository.Received(1).GenerateNewMatchID();
            Assert.AreEqual(matchID, result);
        }
        [Test] 
        public async Task CreateMatchWithThisPlayersIDsAndNumberOfRound_CreateAnswersPerTurnEqualToCategoriesRounds()
        {
            matchRepository.GenerateNewMatchID().Returns("Match");
           
            var playersIDs = new List<string>() { "Player"};
            var numberOfRounds = 2;
            var categoriesPerRound = 2;
            var turnsPerRound = playersIDs.Count;
            var expectedNumberAnswers = numberOfRounds * categoriesPerRound * turnsPerRound;
            var expectedTotalTurns = numberOfRounds * turnsPerRound;
            var timePerTurn = 60;
            
            await createMatch.CreateMatchWithThisPlayersIDsAndNumberOfRounds(playersIDs, numberOfRounds, categoriesPerRound,
                timePerTurn);
            await roundRepository.Received(numberOfRounds).Add(Arg.Any<Round>());
            await turnRepository.Received(expectedTotalTurns).Add(Arg.Any<Turn>());
            await answerRepository.Received(expectedNumberAnswers).Add(Arg.Any<Answer>());

        }
        // A Test behaves as an ordinary method
        [Test] 
        public async Task CreateMatchWithThisPlayersIDsAndNumberOfRounds_OnePlayerID_ReturnMatchWithOneTurnPerRound()
        {
            matchRepository.GenerateNewMatchID().Returns("Match");
            var playersIDs = new List<string>() { "Player"};
            var expectedValue = playersIDs.Count;
            var numberOfRounds = 1;
            var categoriesPerRound = 1;
            var timePerTurn = 60;

            var match = await createMatch.CreateMatchWithThisPlayersIDsAndNumberOfRounds(playersIDs, numberOfRounds, categoriesPerRound,
                timePerTurn);
            await turnRepository.Received(1).Add(Arg.Any<Turn>());
        }
        
       
        
    }
}
