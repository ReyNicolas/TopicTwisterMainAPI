using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests;


public class CheckTurnToPlayTest
{
    private IMatchRepository matchRepository;
    private IRoundRepository roundRepository;
    private ITurnRepository turnRepository;
    private string playerID;
    private CheckTurnToPlay checkTurnToPlay;
    private List<string> categories;
    private List<char> letters;

    [SetUp]
    public void SetUp()
    {
        playerID = "Player";
        matchRepository = Substitute.For<IMatchRepository>();
        roundRepository = Substitute.For<IRoundRepository>();
        turnRepository = Substitute.For<ITurnRepository>();
        checkTurnToPlay = new CheckTurnToPlay(matchRepository, roundRepository, turnRepository);
        categories = new List<string>()
            { "Paises", "Ciudades", "Deportes", "Musica", "Arte", "Ciencia", "Tecnologia", "Naturaleza" };
        letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F' };
    }

    [Test]
    public async Task IsPlayerTurn_IsNotPlayerTurn_ReturnFalse()
    {
        List<string> playersIDs = new List<string>() { "Player2", playerID };
        Match match = GenerateMatchWithRoundsAndTurns("Match1", playersIDs);
        Assert.IsFalse(match.GetActualRound().GetActualTurn().IsPlayer(playerID));
        var result = await checkTurnToPlay.IsPlayerTurn(match.ID, playerID);
        
        Assert.IsFalse(result);
    }
    [Test]
    public async Task IsPlayerTurn_IsPlayerTurn_ReturnTrue()
    {
        string rivalID = "Player2";
        List<string> playersIDs = new List<string>() { playerID, rivalID };
        Match match = GenerateMatchWithRoundsAndTurns("Match1", playersIDs);
        Assert.IsTrue(match.GetActualRound().GetActualTurn().IsPlayer(playerID));
        
        var result = await checkTurnToPlay.IsPlayerTurn(match.ID, playerID);
        
        Assert.IsTrue(result);
    }
    
    private Match GenerateMatchWithRoundsAndTurns(string matchID,List<string> playersIDs)
    {
        
        Match match = new Match(matchID, playersIDs, "", false);
        Round round = new Round(matchID, 1, categories.Take(5).ToList(), letters.First(), "", false, 60);
        List<Turn> turns = playersIDs
            .Select(pid => new Turn(matchID, 1, playersIDs.IndexOf(pid) + 1, pid, 60, false, 0)).ToList();
        round.Turns = turns;
        match.Rounds = new List<Round>() { round };

        matchRepository.Get(matchID).Returns(match);
        roundRepository.GetRoundsFromMatchID(matchID).Returns(match.Rounds);
        turnRepository.GetTurnsFromMatchIDAndRoundID(matchID, round.ID).Returns(turns);
        return match;
    }
    
}