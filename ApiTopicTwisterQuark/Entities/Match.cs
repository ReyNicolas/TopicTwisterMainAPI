namespace ApiTopicTwisterQuark.Entities
{
    public class Match
    {
        public string ID { get; }
        public List<string> PlayersIDs { get; }
    
        public List<Round> Rounds { get; set; }
        private string winnerID;
        private bool tie;
        public string WinnerID
        {
            get { return winnerID; }
        }
    
        public bool Tie {
            get { return tie; }
        }
        public Match(string id, List<string> playerIDs, string winnerID, bool tie)
        {
            ID = id;
            PlayersIDs= playerIDs;
            this.winnerID = winnerID;
            this.tie = tie;
        }

        public bool HasThisPlayerID(string playerID)
        {
            return PlayersIDs.Contains(playerID);
        }

        public bool IsOver()
        {
            return (WinnerID.Length > 1) || (Tie);
        }
        public bool IsPlayerTheWinner(string playerID)
        {
            return (!tie && winnerID== playerID);
        }        

        public string GiveMeRival(string playerID)
        {
            return PlayersIDs.Find(pid => pid != playerID);
        }
        
        public int GetPlayerRoundsWon(string playerID)
        {
            return Rounds.Count(r => r.WinnerID == playerID || r.Tie);
        }
        
        public int GetNumberOfRounds()
        {
            return Rounds.Count;
        }
        public Round GetActualRound()
        {
            return Rounds.FirstOrDefault(round => !round.IsOver(),Rounds.Last());
        }

        public bool AreRoundsOver()
        {
            return Rounds.All(round => round.IsOver());
        }
        public void SetOver()
        {
            if (IsTie())
            {
                tie = true;
            }
            else
            {
                winnerID = GetWinner();
            }
        
        }
        
        private string GetWinner()
        {
            return PlayersIDs.OrderByDescending(pid => GetPlayerRoundsWon(pid)).First();
        }

        private bool IsTie()
        {
            return PlayersIDs.All(pid => GetPlayerRoundsWon(pid) == GetPlayerRoundsWon(PlayersIDs.First()));
        }
        
        
    }
}