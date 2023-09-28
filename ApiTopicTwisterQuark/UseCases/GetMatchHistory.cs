using ApiTopicTwisterQuark.Controllers;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.UseCases;

public class GetMatchHistory
{
    private IMatchRepository matchRepository;
    private IRoundRepository roundRepository;
    public GetMatchHistory(IMatchRepository matchRepository,IRoundRepository roundRepository)
    {
        this.matchRepository = matchRepository;
        this.roundRepository = roundRepository;
    }
        
    public async Task<List<MatchResultDTO>> Execute(string playerID)
    {
        List<Match> playerMatchesOver = await GetMatchesOverForThisPlayer(playerID);
        return await GenerateMatchResultsForPlayer(playerMatchesOver, playerID);
            
    }

    private async Task<List<Match>> GetMatchesOverForThisPlayer(string playerID)
    {
        return (await matchRepository.GetMatches()).Where(
            match => match.HasThisPlayerID(playerID) && match.IsOver()).Reverse().ToList();
    }


    private async Task<List<MatchResultDTO>> GenerateMatchResultsForPlayer(List<Match> matches,string playerID)
    {
        foreach(var match in matches)
        {
            match.Rounds = await roundRepository.GetRoundsFromMatchID(match.ID);
        }
        return  matches.Select(match => CreateMatchResult(match, playerID, match.GiveMeRival(playerID)))
            .ToList();
    }

    private MatchResultDTO CreateMatchResult(Match match, string playerID, string rivalID)
    {
        MatchResultDTO matchResult = new MatchResultDTO(
            match.ID, playerID, rivalID,
            match.GetPlayerRoundsWon(playerID),
            match.GetPlayerRoundsWon(rivalID),
            match.IsPlayerTheWinner(playerID),
            match.Tie
        );

        return matchResult;
    }
}

