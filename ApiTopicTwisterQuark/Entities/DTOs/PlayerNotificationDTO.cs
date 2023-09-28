namespace ApiTopicTwisterQuark.Entities.DTOs;

public class PlayerNotificationDTO
{
    public string notificationID;
    public string playerID;
    public string type;
    public string description;
    

    public PlayerNotificationDTO(string notificationId,string playerID, string type, string description)
    {
        this.notificationID = notificationId;
        this.playerID = playerID;
        this.type = type;
        this.description = description;
    }
}