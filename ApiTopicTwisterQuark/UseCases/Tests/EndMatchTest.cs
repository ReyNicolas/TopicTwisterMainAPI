using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
    public class EndMatchTest
    {
        private IMatchRepository matchRepository;
        private IRoundRepository roundRepository;
        private ITurnRepository turnRepository;
        private IPlayerRepository playerRepository;
        private INotificationService notificationService;
        private ICategoryRepository categoriesRepository;
        private List<string> categoriesNames;
        private List<string> playersIDs;
        private List<char> letters;
        private string matchID;
        private EndMatch endMatch;
        private float time;
        private Player player;
        private int pointsToAdd;
        
        [SetUp]
        public void SetUp()
        {
            matchRepository = Substitute.For<IMatchRepository>();
            roundRepository = Substitute.For<IRoundRepository>();
            turnRepository = Substitute.For<ITurnRepository>();
            playerRepository = Substitute.For<IPlayerRepository>();
            notificationService = Substitute.For<INotificationService>();
            categoriesRepository = Substitute.For<ICategoryRepository>();
            matchID = "Match1";
            matchRepository.GenerateNewMatchID().Returns(matchID);
            
            
            endMatch = new EndMatch(matchRepository,roundRepository, playerRepository, notificationService);
            categoriesNames = new List<string>() { "MODA", "CINE", "MUSICA", "VIDEOJUEGOS", "DEPORTE" };
            playersIDs = new List<string>() { "Player1", "Player2" };
            letters = new List<char>() { 'A','B' };
            time = 60;
            ILettersRepository lettersRepository = Substitute.For<ILettersRepository>();
            categoriesRepository.GetAllCategoriesNames().Returns(categoriesNames);
            lettersRepository.GetAllLetters().Returns(letters);
            player = new Player(playersIDs.First(), 0, "password");
            playerRepository.Get(player.ID).Returns(player);
            pointsToAdd = 1;
        }
        [Test]
        public async Task IsEndOfMatch_RoundsAreNotOver_ReturnFalse()
        {
            //Arrange
            Match match = MatchWithRoundsNotOver();

            //Act
            Assert.IsFalse(match.AreRoundsOver());
            var result = await endMatch.IsEndOfMatch(matchID);

            //Assert
            Assert.IsFalse(result);

        }

        

        [Test]
        public async Task IsEndOfMatch_RoundsAreOver_ReturnTrue()
        {
            //Arrange
            Match match = MatchWithRoundsOver();

            //Act
            Assert.IsTrue(match.AreRoundsOver());
            var result = await endMatch.IsEndOfMatch(matchID);

            //Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task ReturnMatchResultForPlayer_MatchResultDTOReturnPlayerAndRivalID()
        {
            //Arrange
            string expectedPlayerID, expectedRivalID;
            GivenPlayersIDsFromMatch(out expectedPlayerID, out expectedRivalID);

            //Act
            await endMatch.EndThisMatch(matchID);
            var result = await endMatch.ReturnMatchResultForPlayer(matchID, expectedPlayerID);
            var resultPlayerID = result.PlayerID;
            var resultRivalID = result.RivalID;

            //Assert
            Assert.AreEqual(expectedPlayerID, resultPlayerID);
            Assert.AreEqual(expectedRivalID, resultRivalID);
        }


        [Test]
        public async Task ReturnMatchResultForPlayer_MatchResultDTOReturnIfPlayerIsTheWinner()
        {
            //Arrange
            await MatchWhereFirstPlayerIsTheWinner();

            //Act
            var result = await endMatch.ReturnMatchResultForPlayer(matchID, playersIDs.First());
            var resultWinner = result.IsPlayerTheWinner;


            //Assert
            Assert.IsTrue(resultWinner);
        }

        [Test]
         public async Task AddVictoryPointsToWinnerOfThisMatch_AddPoints()
        {
            //Arrange
            await MatchWhereFirstPlayerIsTheWinner();
            int expectedVictoryPoints = player.VictoryPoints + pointsToAdd;

            //Act
            await endMatch.AddVictoryPointsToWinnerOfThisMatch(matchID);


            //Assert
            Assert.AreEqual(expectedVictoryPoints,player.VictoryPoints);
        }


        [Test]
        public async Task ReturnMatchResultForPlayer_MatchResultDTOReturnIfTie()
        {
            //Arrange
            await MatchWherePlayersTie();

            //Act
            var result = await endMatch.ReturnMatchResultForPlayer(matchID, playersIDs.First());
            var resultTie = result.IsTie;

            //Assert
            Assert.IsTrue(resultTie);
        }


        [Test]
        public async Task ReturnMatchResultForPlayer_MatchResultDTOReturnPlayerAndRivalRoundsWonCount()
        {
            //Arrange
            var playerID = playersIDs.First();
            var rivalID = playersIDs.Last();
            Match match = await MatchWherePlayerWonRounds(playerID);
            var expectedPlayerCount = match.GetPlayerRoundsWon(playerID);
            var expectedRivalCount = match.GetPlayerRoundsWon(rivalID);

            //Act
            var result = await endMatch.ReturnMatchResultForPlayer(matchID, playerID);
            var resultPlayerCount = result.PlayerRoundsWon;
            var resultRivalCount = result.RivalRoundsWon;

            //Assert
            Assert.AreEqual(expectedPlayerCount, resultPlayerCount);
            Assert.AreEqual(expectedRivalCount, resultRivalCount);
        }


        private Match MatchWithRoundsNotOver()
        {
            Match match = new Match(matchID, playersIDs, "", false);
            Round round = new Round(matchID, 1, categoriesNames.Take(5).ToList(), letters.First()
                , "", false, time);
            match.Rounds = new List<Round>() { round };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(match.Rounds);
            return match;
        }
        private Match MatchWithRoundsOver()
        {
            Match match = new Match(matchID, playersIDs, "", false);
            Round round = new Round(matchID, 1, categoriesNames.Take(5).ToList(), letters.First()
                , "", true, time);
            match.Rounds = new List<Round>() { round };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(match.Rounds);
            return match;
        }
        private void GivenPlayersIDsFromMatch(out string expectedPlayerID, out string expectedRivalID)
        {
            Match match = new Match(matchID, playersIDs, "", false);
            Round round = new Round(matchID, 1, categoriesNames.Take(5).ToList(), letters.First()
                , "", true, time);
            match.Rounds = new List<Round>() { round };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(match.Rounds);
            expectedPlayerID = playersIDs.First();
            expectedRivalID = playersIDs.Last();
        }
        private async Task MatchWhereFirstPlayerIsTheWinner()
        {
            Match match = new Match(matchID, playersIDs, "", false);
            Round round1 = new Round(matchID, 1, categoriesNames.GetRange(0, 2), 'A', playersIDs.First(), false, 60);
            Round round2 = new Round(matchID, 2, categoriesNames.GetRange(2, 2), 'B', playersIDs.First(), false, 60);
            List<Round> rounds = new List<Round>() { round1, round2 };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(rounds);
            await endMatch.EndThisMatch(matchID);
        }
        private async Task MatchWherePlayersTie()
        {
            Match match = new Match(matchID, playersIDs, "", false);
            List<Round> rounds = new List<Round>()
            {
                new Round(matchID, 1, new List<string>(), 'A', playersIDs[0], false, 60),
                new Round(matchID, 2, new List<string>(), 'B', playersIDs[1], false, 60)
            };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(rounds);
            await endMatch.EndThisMatch(matchID);
        }
        private async Task<Match> MatchWherePlayerWonRounds(string playerID)
        {
            Match match = new Match(matchID, playersIDs, "", false);
            List<Round> rounds = new List<Round>()
            {
                new Round(matchID, 1, new List<string>(), 'A', playerID, false, 60),
                new Round(matchID, 2, new List<string>(), 'B', playerID, false, 60)
            };
            matchRepository.Get(matchID).Returns(match);
            roundRepository.GetRoundsFromMatchID(matchID).Returns(rounds);
            match.Rounds = rounds;
            await endMatch.EndThisMatch(matchID);
            return match;
        }

    }
}