using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Utils;

namespace ApiTopicTwisterQuark.UseCases
{
    public class SendEndMatchNotificationsToPlayers
    {
        IMatchRepository matchRepository;
        INotificationService notificationService;
        AllForGameNotificationsIDs notificationsIDs;

        public SendEndMatchNotificationsToPlayers(IMatchRepository matchRepository,INotificationService notificationService, AllForGameNotificationsIDs notificationsIDs)
        {
            this.notificationsIDs = notificationsIDs;
            this.matchRepository = matchRepository;
            this.notificationService = notificationService;
        }

        public async Task Execute(string matchID)
        {
            Match match = await matchRepository.Get(matchID);
            foreach (var playerID in match.PlayersIDs)
            {
                await notificationService.AddPlayerForGameNotification(playerID,notificationsIDs.historialChange);
            }
        }
        
    }

    



}