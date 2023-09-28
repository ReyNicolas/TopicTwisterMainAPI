namespace ApiTopicTwisterQuark.Entities.DTOs;

[Serializable]
public class PlayerData
{
    public string id { get; set; }
    public string password { get; set; }

    public int victoryPoints { get; set; }

    public PlayerData(string id, string password, int victoryPoints)
    {
        this.id = id;
        this.password = password;
        this.victoryPoints = victoryPoints;
    }
}