using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using ApiTopicTwisterQuark.Utils;

namespace ApiTopicTwisterQuark.Services;

public class NotificationService : INotificationService
{
    private TopicTwisterContext context;
    private AllForGameNotificationsIDs allForGameNotificationsID;
    private string rematchID = "REMATCH";

    public NotificationService(TopicTwisterContext context)
    {
        this.allForGameNotificationsID = new AllForGameNotificationsIDs();
        this.context = context;
    }   

    public async Task<List<ForGameNotificationDTO>> GetPlayerForGameNotification(string playerId)
    {
        var forGameNotifications = await context.ForGamesNotifications
            .Where(gn => gn.PlayerID == playerId)
            .Select(gn => new ForGameNotificationDTO(gn.NotificationID,gn.PlayerID))
            .ToListAsync();
        return forGameNotifications;
    }

    public async Task RemovePlayerForGameNotification(string playerId,string notificationID)
    {
        foreach (var playerForGameNotificationEntity in context.ForGamesNotifications)
        {
            if (playerForGameNotificationEntity.PlayerID == playerId && playerForGameNotificationEntity.NotificationID == notificationID)
                context.ForGamesNotifications.Remove(playerForGameNotificationEntity);
        }
        await context.SaveChangesAsync();
    }
   

    public async Task<List<PlayerRematchNotificationDTO>> GetPlayerRematchNotifications(string playerId)
    {
        var rematchNotifications = await context.RematchesNotifications
            .Where(rn =>  rn.PlayerID == playerId)
            .Select(rn => new PlayerRematchNotificationDTO(rn.RivalID, rn.NotificationID, rn.PlayerID))
            .ToListAsync();
        return rematchNotifications;
    }

    public async Task AddPlayerRematchNotification(string senderPlayerID, string receiverPlayerID)
    {
        if (context.RematchesNotifications.Any(rn => rn.PlayerID == receiverPlayerID && rn.RivalID == senderPlayerID))
            return;
        var playerRematchNotificationEntity = new RematchNotificationEntity();
        playerRematchNotificationEntity.NotificationID = rematchID;
        playerRematchNotificationEntity.PlayerID = receiverPlayerID;
        playerRematchNotificationEntity.RivalID = senderPlayerID;
        await context.RematchesNotifications.AddAsync(playerRematchNotificationEntity);
        await context.SaveChangesAsync();

        await AddPlayerForGameNotification(receiverPlayerID, allForGameNotificationsID.rematchNotifications);
    }
    
   
    public async Task AddPlayerForGameNotification(string playerId, string notificationID)
    {
        if (context.ForGamesNotifications.Any(gn => gn.PlayerID == playerId && gn.NotificationID == notificationID))
            return;
        var forGameNotificationEntity = new ForGameNotificationEntity();
        forGameNotificationEntity.NotificationID = notificationID;
        forGameNotificationEntity.PlayerID = playerId;
        await context.ForGamesNotifications.AddAsync(forGameNotificationEntity);
        await context.SaveChangesAsync();
    }
 
    public async Task RemovePlayerRematchNotification(string playerID, string rivalID)
    {
        var playerRematchNotificationEntity = await context.RematchesNotifications.FirstAsync(rn => rn.PlayerID == playerID && rn.RivalID == rivalID);
        if(playerRematchNotificationEntity!= null)
        {
            context.RematchesNotifications.Remove(playerRematchNotificationEntity);
            await context.SaveChangesAsync();
        }
        await AddPlayerForGameNotification(playerID, allForGameNotificationsID.rematchNotifications);
    }
}
