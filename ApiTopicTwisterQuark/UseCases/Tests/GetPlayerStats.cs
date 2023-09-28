using ApiTopicTwisterQuark.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
    public class GetPlayerStatsTest
    {
        private IPlayerService playerService;
        private Entities.Player player;
        private string playerID;
        private string password;
        private GetPlayerStats getPlayerStats;
        private int victoryPoints;

        [SetUp]
        public void Setup()
        {
            victoryPoints = 10;
            playerService = Substitute.For<IPlayerService>();
            playerID = "Player";
            password = "password";
            player = new Entities.Player(playerID,victoryPoints,password);
            playerService.Get(playerID).Returns(player);
            getPlayerStats = new GetPlayerStats(playerService);
        }

        [Test]
        public async Task Execute_GetPlayer_FromPlayerService()
        {
            //Arrange

            //Act
            await getPlayerStats.Execute(playerID);
            //Arrange
            await playerService.Received().Get(playerID);
        }

        [Test]
        public async Task Execute_ReturnsStatsWithVictoryPoints()
        {
            //Arrange
            var expectedVP = player.VictoryPoints;
            //Act
            var result = (await getPlayerStats.Execute(playerID)).victoryPoints;
            //Assert
            Assert.AreEqual(expectedVP, result);
        }
    }
}
