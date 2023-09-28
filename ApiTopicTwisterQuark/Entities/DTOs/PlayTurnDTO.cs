namespace ApiTopicTwisterQuark.Entities.DTOs
{
    [Serializable]
    public class PlayTurnDTO
    {
        public string MatchID;
        public int RoundID;
        public int NumberOfRounds;
        public string PlayerID;
        public float Time;
        public List<string> Categories;
        public List<string> Words;
        public char Letter;

        public PlayTurnDTO(string matchID, int roundID, int numberOfRounds,string playerID, float time, List<string> categories, List<string> words, char letter)
        {
            MatchID = matchID;
            RoundID = roundID;
            NumberOfRounds = numberOfRounds;
            PlayerID = playerID;
            Time = time;
            Categories = categories;
            Words = words;
            Letter = letter;
        }
    }
}