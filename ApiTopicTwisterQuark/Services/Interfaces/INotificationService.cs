using ApiTopicTwisterQuark.Entities.DTOs;

namespace ApiTopicTwisterQuark.Services;

public interface INotificationService
{
    Task<List<ForGameNotificationDTO>> GetPlayerForGameNotification(string playerId);
    Task<List<PlayerRematchNotificationDTO>> GetPlayerRematchNotifications(string playerId);

    Task RemovePlayerForGameNotification(string playerId, string notificationID);
   Task AddPlayerForGameNotification(string playerID, string notificationID);
    Task AddPlayerRematchNotification(string senderPlayerID,string receiverPlayerID);
    Task RemovePlayerRematchNotification(string playerID, string rivalID);
}



