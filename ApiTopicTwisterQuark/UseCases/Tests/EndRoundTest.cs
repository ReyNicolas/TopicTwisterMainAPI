using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
    public class EndRoundTest
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        private ICategoryRepository categoriesRepository;
        private EndRound endRound;
        private List<string> categoriesNames;
        private List<string> playersIDs;
        private List<char> letters;
        private string matchID;
        private Category categoryMock;
        Round round;
        int roundID;
        string playerID;
        string rivalID;


        [SetUp]
        public void SetUp()
        {
            matchRepository = Substitute.For<IMatchRepository>();
            roundRepository = Substitute.For<IRoundRepository>();
            turnRepository = Substitute.For<ITurnRepository>();
            answerRepository = Substitute.For<IAnswerRepository>();
            categoriesRepository = Substitute.For<ICategoryRepository>();
            matchID = "Match1";
            matchRepository.GenerateNewMatchID().Returns(matchID);
            categoryMock = new Category("any", new List<string>());      
           
            endRound = new EndRound(roundRepository, turnRepository, answerRepository,categoriesRepository);
            categoriesNames = new List<string>() { "MODA", "CINE", "MUSICA", "VIDEOJUEGOS", "DEPORTE" };
            playersIDs = new List<string>() { "Player1", "Player2" };
            letters = new List<char>() { 'A' };            
            ILettersRepository lettersRepository = Substitute.For<ILettersRepository>();
            categoriesRepository.GetAllCategoriesNames().Returns(categoriesNames);
            lettersRepository.GetAllLetters().Returns(letters);
            roundID = 1;
            round = new Round(matchID, roundID, categoriesNames, letters.First(), "", false, 60);
             playerID = playersIDs[0];
             rivalID = playersIDs[1];

        }

        [Test]
        public async Task IsActualRoundOver_actualRoundsTurnsAreNotOver_ReturnFalse()
        {
            //Arrange
            RoundWithTurnsNotOver();

            //Act
            var result = await endRound.IsActualRoundOver(matchID, roundID);

            //Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task IsActualRoundOver_actualRoundsTurnsAreOver_ReturnTrue()
        {
            //arrange
            List<Turn> turns = TurnsFromRoundWithAllTurnsOver();

            //act
            Assert.IsTrue(turns.All(t => t.Finish));
            var result = await endRound.IsActualRoundOver(matchID, roundID);
            //assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task ReturnRoundResultForPlayer()
        {
            await RoundWithTurnsOverAndTiePlayerWins();

            await endRound.EndThisRound(matchID, roundID);
            var result = await endRound.ReturnRoundResultForPlayer(matchID, roundID, playerID);

            Assert.IsTrue(result.Tie);
        }

        private async Task RoundWithTurnsOverAndTiePlayerWins()
        {
            int playerTurnID = 1;
            int rivalTurnID = 2;
            Turn playerTurn = new Turn(matchID, roundID, playerTurnID, playerID, 40, true, 2);
            Turn rivalTurn = new Turn(matchID, roundID, rivalTurnID, rivalID, 30, true, 3);
            List<Turn> turns = new List<Turn>()
            {
                playerTurn,rivalTurn
            };
            roundRepository.Get(matchID, roundID).Returns(round);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(new List<Round>() { round });
            turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID).Returns(turns);
            turnRepository.Get(matchID, roundID, playerTurnID).Returns(playerTurn);
            turnRepository.Get(matchID, roundID, rivalTurnID).Returns(rivalTurn);
            categoriesRepository.GetCategoryByName(Arg.Any<string>()).Returns(categoryMock);
            EndTurn endTurn = new EndTurn(categoriesRepository, turnRepository, answerRepository);
            DataApplication playerDataApplication = new DataApplication(matchID, roundID, playerTurnID, 3, playerID);
            DataApplication rivalDataApplication = new DataApplication(matchID, roundID, rivalTurnID, 3, rivalID);
            List<Answer> playerAnswers = new List<Answer>();
            List<Answer> rivalAnswers = new List<Answer>();
            foreach (var categoryName in categoriesNames)
            {

                playerAnswers.Add(new Answer(matchID, roundID, 1, categoryName, "", round.Letter));
                rivalAnswers.Add(new Answer(matchID, roundID, 2, categoryName, "", round.Letter));
            }
            answerRepository.GetAnswersFromTurn(matchID, roundID, 1).Returns(playerAnswers);
            answerRepository.GetAnswersFromTurn(matchID, roundID, 2).Returns(rivalAnswers);
            await endTurn.EndThisTurn(playerDataApplication, 60);
            await endTurn.EndThisTurn(rivalDataApplication, 60);
        }

        private void RoundWithTurnsNotOver()
        {
            List<Turn> turns = new List<Turn>()
            {
                new Turn(matchID, roundID, 1, playersIDs.First(), 60, false, 0),
                new Turn(matchID, roundID, 2, playersIDs.Last(), 60, false, 0),
            };
            roundRepository.Get(matchID, roundID).Returns(round);
            turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID).Returns(turns);
        }
        
        private List<Turn> TurnsFromRoundWithAllTurnsOver()
        {
            List<Turn> turns = new List<Turn>()
            {
                new Turn(matchID, roundID, 1, playersIDs.First(), 40, true, 2),
                new Turn(matchID, roundID, 2, playersIDs.Last(), 30, true, 3),
            };
            roundRepository.Get(matchID, roundID).Returns(round);
            turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, roundID).Returns(turns);
            return turns;
        }
        
    }
}