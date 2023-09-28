using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests;


public class GetMatchHistoryTest
{
    private IMatchRepository matchRepository;
    private IRoundRepository roundRepository;
    private string playerID;
    private GetMatchHistory getMatchHistory;
    private List<string> categories;
    private List<char> letters;
    
    [SetUp]
    public void SetUp()
    {
        playerID = "Player";
        matchRepository = Substitute.For<IMatchRepository>();
        roundRepository = Substitute.For<IRoundRepository>();
        getMatchHistory = new GetMatchHistory(matchRepository, roundRepository);
        categories = new List<string>()
            { "Paises", "Ciudades", "Deportes", "Musica", "Arte", "Ciencia", "Tecnologia", "Naturaleza" };
        letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F' };
    }
    [Test]
    public async Task Execute_playerIDDoesntHaveMatchesOver_ReturnEmptyListMatchResult()
    {
        //Arrange
        MatchesOverButNotForPlayer();
        //Act
        var result = await getMatchHistory.Execute(playerID);
        //Assert
        Assert.IsEmpty(result);
    }


    [Test]
    public async Task Execute_playerIDHasMatchesOver_ReturnMatchResultForEachMatchOver()
    {
        int expectedCount = NumberOfMatchesOverForPlayer();

        var playerMatchesResults = await getMatchHistory.Execute(playerID);
        var result = playerMatchesResults.Count;

        Assert.AreEqual(expectedCount, result);
    }

    private int NumberOfMatchesOverForPlayer()
    {
        List<string> notplayersIDs = new List<string>() { "Player1", "Player2" };
        Match match1 = GenerateMatchOverWithRounds("Match1", notplayersIDs);
        Match match2 = new Match("Match2", new List<string>() { playerID, notplayersIDs.First() }, "", false);
        Match match3 = GenerateMatchOverWithRounds("Match3", new List<string>() { playerID, notplayersIDs.First() });
        Match match4 = GenerateMatchOverWithRounds("Match4", new List<string>() { playerID, notplayersIDs.First() });
        List<Match> matches = new List<Match>() { match1, match2, match3, match4 };
        matchRepository.GetMatches().Returns(matches);
        var expectedCount = matches.Count(m => m.PlayersIDs.Contains(playerID) && m.IsOver());
        return expectedCount;
    }

    private Match GenerateMatchOverWithRounds(string matchID,List<string> playersIDs)
    {
        
        Match match = new Match(matchID, playersIDs, playersIDs.First(), false);
        Round round = new Round(matchID, 1, categories.Take(5).ToList(), letters.First(), playersIDs.First(), false, 60);
        match.Rounds = new List<Round>() { round };
        roundRepository.GetRoundsFromMatchID(matchID).Returns(match.Rounds);
        return match;
    }
    private void MatchesOverButNotForPlayer()
    {
        List<string> notplayersIDs = new List<string>() { "Player1", "Player2" };
        Match match = GenerateMatchOverWithRounds("Match1", notplayersIDs);
        Match match2 = new Match("Match2", new List<string>() { playerID, notplayersIDs.First() }, "", false);
        matchRepository.GetMatches().Returns(new List<Match>() { match, match2 });
    }
}