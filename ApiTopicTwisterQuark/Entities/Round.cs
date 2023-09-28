namespace ApiTopicTwisterQuark.Entities;

public class Round
{
    public string MatchID { get; }
    public int ID { get; }
    public List<string> Categories { get; }
    public char Letter { get; }
    public float InitialTimePerTurn { get; }
    public List<Turn> Turns { get; set; }
    private string winnerID;
    private bool tie;

    public string WinnerID
    {
        get { return winnerID; }
    }

    public bool Tie
    {
        get { return tie; }
    }

    public Round(string matchID, int id, List<string> categories, char letter, string winnerID, bool tie, float time)
    {
        MatchID = matchID;
        ID = id;
        Categories = categories;
        Letter = letter;
        this.winnerID = winnerID;
        this.tie = tie;
        InitialTimePerTurn = time;
    }

    public bool IsPlayerTheWinner(string playerID) => WinnerID == playerID;

    public void SetOver()
    {
        if (IsTie())
        {
            tie = true;
        }
        else if (TieCorrectCount())
        {
            winnerID = MostPlayerTimeLeft();
        }
        else
        {
            winnerID = MostPlayerCorrectCount();
        }
    }
    public string GetRivalIDFromTurns(string playerID) => 
        Turns.Find(t => t.PlayerID != playerID).PlayerID;

    public Turn GetActualTurn() => 
        Turns.FirstOrDefault(turn => !turn.Finish,Turns.Last());

    public bool IsOver() => 
        (WinnerID.Length > 1) || (Tie);

    public bool AreTurnsOver() => 
        Turns.All(turn => turn.Finish);

    public Turn GetTurnOfPlayerID(string playerId) => 
        Turns.Find(t => t.PlayerID == playerId);

    private bool IsTie() => 
        TieWithAnswers() || AllPlayersZeroAnswers();

    private bool TieWithAnswers() => 
        TieCorrectCount() && TieTimeLeft();

    private bool AllPlayersZeroAnswers()=> 
        Turns.All(t => t.CorrectCount == 0);
    

    private bool TieCorrectCount()=> 
        Turns.All(t => t.CorrectCount == Turns.First().CorrectCount);
    
    private bool TieTimeLeft() => 
        Turns.All(t => t.Time == Turns.First().Time);

    private string MostPlayerTimeLeft() => 
        Turns.OrderByDescending(t => t.Time).First().PlayerID;

    private string MostPlayerCorrectCount() => 
        Turns.OrderByDescending(t => t.CorrectCount).First().PlayerID;



   
}