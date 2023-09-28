
namespace ApiTopicTwisterQuark.Entities
{
    public class Player
    {
        public string ID { get; }
        int victoryPoints;
        private string password;
        public string Password
        {
            get { return password; }
        }

        public int VictoryPoints
        {
            get { return victoryPoints; }
        }
        public Player(string iD, int victoryPoints,string password)
        {
            ID = iD;
            this.victoryPoints = victoryPoints;
            this.password = password;
        }

        public void AddVictoryPoints(int value)
        {
            victoryPoints += value;
        }

    
    }
}
