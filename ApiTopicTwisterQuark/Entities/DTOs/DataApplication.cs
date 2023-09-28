namespace ApiTopicTwisterQuark.Entities.DTOs
{
    [Serializable]
    public class DataApplication
    {
        public string MatchID;
        public int RoundID;
        public int TurnID;
        public int NumberOfRounds;
        public string PlayerID;
        
        
        public DataApplication(string matchID, int roundID,int turnID, int numberOfRounds , string playerID)
        {
            MatchID = matchID;
            RoundID = roundID;
            TurnID = turnID;
            NumberOfRounds = numberOfRounds;
            PlayerID = playerID;
        }
    }
}