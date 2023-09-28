namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class MatchResultDTO
    {
        public string MatchID;
        public string PlayerID;
        public string RivalID;
        public int PlayerRoundsWon;
        public int RivalRoundsWon;
        public bool IsPlayerTheWinner;
        public bool IsTie;

        public MatchResultDTO(string matchID, string playerID, string rivalID, int playerRoundsWon, int rivalRoundsWon, bool isPlayerTheWinner, bool isTie)
        {
            MatchID = matchID;
            PlayerID = playerID;
            RivalID = rivalID;
            PlayerRoundsWon = playerRoundsWon;
            RivalRoundsWon = rivalRoundsWon;
            IsPlayerTheWinner = isPlayerTheWinner;
            IsTie = isTie;
        }
    }
}