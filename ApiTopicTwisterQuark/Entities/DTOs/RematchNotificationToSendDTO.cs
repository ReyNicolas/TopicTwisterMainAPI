namespace ApiTopicTwisterQuark.Entities.DTOs;

public class RematchNotificationToSendDTO
{
    public string receiverPlayerID{ get; set; }
    public string senderPlayerID { get; set; }

    public RematchNotificationToSendDTO(string senderPlayerID, string receiverPlayerID)
    {
        this.senderPlayerID = senderPlayerID;
        this.receiverPlayerID = receiverPlayerID;
    }

}