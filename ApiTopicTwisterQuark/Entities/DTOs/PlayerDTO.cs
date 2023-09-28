namespace ApiTopicTwisterQuark.Entities.DTOs;

public class PlayerDTO
{
    public string id;
    public string password;
    public int victoryPoints;


    public PlayerDTO(string id, string password, int victoryPoints)
    {
        this.id = id;
        this.password = password;
        this.victoryPoints = victoryPoints;
    }

}