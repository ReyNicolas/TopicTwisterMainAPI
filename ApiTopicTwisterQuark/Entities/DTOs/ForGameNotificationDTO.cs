namespace ApiTopicTwisterQuark.Entities.DTOs;

public class ForGameNotificationDTO
{
    public string notificationID;
    public string playerID;

    public ForGameNotificationDTO(string notificationId, string playerId)
    {
        notificationID = notificationId;
        playerID = playerId;
    }
}
