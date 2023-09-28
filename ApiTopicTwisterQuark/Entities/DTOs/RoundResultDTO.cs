namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class RoundResultDTO
    {
        public string PlayerID;
        public string RivalID;
        public int RoundID;
        public int NumberOfRounds;
        public char Letter;
        public List<AnswerComparerDTO> answersComparer;
        public int PlayerCorrectsCount;
        public float PlayerTimeLeft;
        public int RivalCorrectsCount;
        public float RivalTimeLeft;
        public bool IsPlayerTheWinner;
        public bool Tie;

        public RoundResultDTO(string playerID, string rivalID, int roundID, int numberOfRounds, char letter,
            List<AnswerComparerDTO> answersComparer, int playerCorrectsCount, float playerTimeLeft,
            int rivalCorrectsCount, float rivalTimeLeft, bool  isPlayerTheWinner, bool tie)
        {
            PlayerID = playerID;
            RivalID = rivalID;
            RoundID = roundID;
            NumberOfRounds = numberOfRounds;
            Letter = letter;
            this.answersComparer = answersComparer;
            PlayerCorrectsCount = playerCorrectsCount;
            PlayerTimeLeft = playerTimeLeft;
            RivalCorrectsCount = rivalCorrectsCount;
            RivalTimeLeft = rivalTimeLeft;
            IsPlayerTheWinner = isPlayerTheWinner;
            Tie = tie;
        }
    }
}