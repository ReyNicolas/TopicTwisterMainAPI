namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class StartTurnDTO
    {
        public int RoundID;
        public int NumberOfRounds;
        public float Time;
        public List<string> Categories;
        public char Letter;

        public StartTurnDTO(int roundID, int numberOfRounds, float time, List<string> categories, char letter)
        {
            RoundID = roundID;
            NumberOfRounds = numberOfRounds;
            Time = time;
            Categories = categories;
            Letter = letter;
        }
    }
}