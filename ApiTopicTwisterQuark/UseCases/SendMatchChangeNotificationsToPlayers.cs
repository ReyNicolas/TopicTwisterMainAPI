using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Utils;

namespace ApiTopicTwisterQuark.UseCases
{
    public class SendMatchChangeNotificationsToPlayers
    {
        IMatchRepository matchRepository;
        INotificationService notificationService;
        AllForGameNotificationsIDs allForGameNotificationsIDs;

        public SendMatchChangeNotificationsToPlayers(IMatchRepository matchRepository, INotificationService notificationService, AllForGameNotificationsIDs allForGameNotificationsIDs)
        {
            this.allForGameNotificationsIDs = allForGameNotificationsIDs;
            this.matchRepository = matchRepository;
            this.notificationService = notificationService;
        }

        public async Task Execute(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            foreach (var playerID in match.PlayersIDs)
            {
                await notificationService.AddPlayerForGameNotification(playerID, allForGameNotificationsIDs.matchesChange);
            }
        }

    }

    
}