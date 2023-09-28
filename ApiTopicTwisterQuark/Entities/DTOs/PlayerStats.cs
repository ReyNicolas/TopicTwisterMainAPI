namespace ApiTopicTwisterQuark.Entities.DTOs
{
    [Serializable]
    public class PlayerStats
    {
        public int victoryPoints;
        public int currency;

        public PlayerStats(int victoryPoints, int currency)
        {
            this.victoryPoints = victoryPoints;
            this.currency = currency;

        }

    }
}