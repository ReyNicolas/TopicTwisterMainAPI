using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests;


public class GetMatchesToPlayTest
{
    private IMatchRepository matchRepository;
    private IRoundRepository roundRepository;
    private ITurnRepository turnRepository;
    private string playerID;
    private GetMatchesToPlayInfo getMatchesToPlay;
    private List<string> categories;
    private List<char> letters;

    [SetUp]
    public void SetUp()
    {
        playerID = "Player";
        matchRepository = Substitute.For<IMatchRepository>();
        roundRepository = Substitute.For<IRoundRepository>();
        turnRepository = Substitute.For<ITurnRepository>();
        getMatchesToPlay = new GetMatchesToPlayInfo(matchRepository, roundRepository, turnRepository);
        categories = new List<string>()
            { "Paises", "Ciudades", "Deportes", "Musica", "Arte", "Ciencia", "Tecnologia", "Naturaleza" };
        letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F' };
    }
    [Test]
    public async Task ReturnsMatchesInfo_playerIDDoesntHaveMatches_ReturnEmptyListMatchInfo()
    {
        PlayerWithoutMatchesToPLay();

        var result = await getMatchesToPlay.Execute(playerID);

        Assert.IsEmpty(result);
    }


    [Test]
    public async Task ReturnsMatchesInfo_playerIDHasOneMatchToPlay_ReturnListWithOneMatchInfo()
    { 
        //Arrange
        int expectedCount = OneMatchToPlayForPlayer();

       //Act
        var playerMatchesInfos = await getMatchesToPlay.Execute(playerID);
        var result = playerMatchesInfos.Count;

        //Assert
        Assert.AreEqual(expectedCount, result);
    }



    [Test]
    public async Task ReturnsMatchesInfo_playerIDHasThreeMatchesToPlay_ReturnListWithThreeMatchInfos()
    {
        int expectedCount = ThreeMatchesToPlayForPlayer();

        var playerMatchesInfos = await getMatchesToPlay.Execute(playerID);
        var result = playerMatchesInfos.Count;

        Assert.AreEqual(expectedCount, result);
    }

    private int ThreeMatchesToPlayForPlayer()
    {
        List<string> notplayersIDs = new List<string>() { "Player1", "Player2" };
        List<string> playersIDs = new List<string>() { playerID, "Player2" };
        Match match1 = GenerateMatchWithRoundsAndTurns("Match1", notplayersIDs);
        Match match2 = GenerateMatchWithRoundsAndTurns("Match2", playersIDs);
        Match match3 = GenerateMatchWithRoundsAndTurns("Match3", playersIDs);
        Match match4 = GenerateMatchWithRoundsAndTurns("Match4", playersIDs);
        Match match5 = new Match("Match5", playersIDs, playerID, false);
        List<Match> matches = new List<Match>() { match1, match2, match3, match4, match5 };
        var expectedCount = matches.Count(m => m.PlayersIDs.Contains(playerID) && !m.IsOver());
        matchRepository.GetMatches().Returns(matches);
        Assert.AreEqual(3, expectedCount);
        return expectedCount;
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
    private void PlayerWithoutMatchesToPLay()
    {
        List<string> notplayersIDs = new List<string>() { "Player1", "Player2" };
        Match match = new Match("Match", notplayersIDs, "", false);
        matchRepository.GetMatches().Returns(new List<Match>() { match });
    }
    private int OneMatchToPlayForPlayer()
    {
        List<string> notplayersIDs = new List<string>() { "Player1", "Player2" };
        List<string> playersIDs = new List<string>() { playerID, "Player2" };
        Match match1 = GenerateMatchWithRoundsAndTurns("Match1", notplayersIDs);
        Match match2 = GenerateMatchWithRoundsAndTurns("Match2", playersIDs);
        List<Match> matches = new List<Match>() { match1, match2 };
        var expectedCount = matches.Count(m => m.PlayersIDs.Contains(playerID));
        matchRepository.GetMatches().Returns(matches);
        return expectedCount;
    }
    
}