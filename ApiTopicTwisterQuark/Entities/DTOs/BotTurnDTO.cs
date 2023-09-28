namespace ApiTopicTwisterQuark.Entities.DTOs
{
    [Serializable]
    public class BotTurnDTO
    {
        public string MatchID;
        public int RoundID;
        public int TurnID;
        public List<string> Categories;
        public char Letter;
        public float Time;

        public BotTurnDTO(string matchID, int roundID, int turnID, List<string> categories, char letter, float time)
        {
            MatchID = matchID;
            RoundID = roundID;
            TurnID = turnID;
            Categories = categories;
            Letter = letter;
            Time = time;
        }
    }
}