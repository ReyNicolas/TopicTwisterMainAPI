using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Utils;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
    public class SendEndMatchNotificationsToPlayersTest
    {
        SendEndMatchNotificationsToPlayers sendEndMatchNotificationsToPlayers;
        IMatchRepository matchRepository;
        INotificationService notificationService;
        AllForGameNotificationsIDs allForGameNotificationsIDs;
        Match match;
        string matchID;
        List<string> playersIDs;

        [SetUp]
        public void SetUp()
        {
            matchRepository = Substitute.For<IMatchRepository>();
            notificationService = Substitute.For<INotificationService>();
            allForGameNotificationsIDs = new AllForGameNotificationsIDs();
            matchID = "Match";
            playersIDs = new List<string>() { "player1","player2"};
            match = new Match(matchID, playersIDs, playersIDs.First(), false);
            sendEndMatchNotificationsToPlayers = 
                new SendEndMatchNotificationsToPlayers(matchRepository, notificationService, allForGameNotificationsIDs);
            matchRepository.Get(matchID).Returns(match);
        }

        [Test]
        public async Task Execute_Should_Get_Match_From_Repository()
        {
            //arrange

            //Act
            await sendEndMatchNotificationsToPlayers.Execute(matchID);
            //Arrange
            await matchRepository.Received().Get(matchID);
        }

        [Test]
        public async Task Execute_Should_Add_ForGameNotification_Foreach_Player()
        {
            //arrange
            //Act
            await sendEndMatchNotificationsToPlayers.Execute(matchID);
            //Arrange
            foreach(var playerId in playersIDs)
            {
               await notificationService.Received().AddPlayerForGameNotification(playerId,allForGameNotificationsIDs.historialChange);
            }
        }

    }
}
