namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class PlayerRematchNotificationDTO
    {
        public string rivalID;
        public string notificationID;
        public string playerID;


        public PlayerRematchNotificationDTO(string rivalID, string notificationId, string playerID)
        {
            this.rivalID = rivalID;
            this.notificationID = notificationId;
            this.playerID = playerID;

        }
    }
}