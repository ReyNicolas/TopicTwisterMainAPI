namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class MatchConfiguration
    {
        public List<string> PlayersIDs { get; set; }
        public int NumberOfRounds { get; set; }
        public int CategoriesPerRound { get; set; }
        public float TimePerTurn { get; set; }

    }
}