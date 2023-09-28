using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTopicTwisterQuark.Repositories;



public class MatchRepository : IMatchRepository
{
    private TopicTwisterContext context;

    public MatchRepository(TopicTwisterContext context)
    {
        this.context = context;
    }

    public async Task<List<Match>> GetMatches()
    {
        List<MatchEntity> matchEntities = await context.Matches.ToListAsync();
        List<Match> matches = new List<Match>();
        foreach (var matchEntity in matchEntities)
        {
            matches.Add(await Get(matchEntity.ID));
        }

        return matches;
    }
    
    public async Task<Match> Get(string matchID)
    {
        MatchEntity matchEntity = await context.Matches.FirstAsync(m => m.ID == matchID);
        List<string> playersIDs = await context.MatchesPlayers.Where(
            mp => mp.MatchID == matchID).Select(mp => mp.PlayerID).ToListAsync();
        Match match = new Match(matchEntity.ID, playersIDs, matchEntity.WinnerID,matchEntity.Tie);
        return match;
    }

    public async Task<string> GenerateNewMatchID()
    {
        return "MatchMock" + ( await context.Matches.CountAsync()+1);
    }

    public async Task<bool> Add(Match match)
    {
        MatchEntity matchEntity = new MatchEntity();
        matchEntity.ID = match.ID;
        matchEntity.Tie = match.Tie;
        matchEntity.WinnerID = match.WinnerID;
        await context.Matches.AddAsync(matchEntity);

        foreach (string playerID in match.PlayersIDs)
        {
            MatchPlayerEntity matchPlayerEntity = new MatchPlayerEntity();
            matchPlayerEntity.MatchID = match.ID;
            matchPlayerEntity.PlayerID = playerID;
            await context.MatchesPlayers.AddAsync(matchPlayerEntity);
        }    
        
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(Match match)
    {
        MatchEntity matchEntity = await context.Matches.FirstAsync(m => m.ID == match.ID);
        matchEntity.WinnerID = match.WinnerID;
        matchEntity.Tie = match.Tie;

        await context.SaveChangesAsync();

        return true;
    }

    public Task<bool> Delete(string matchID)
    {
        throw new NotImplementedException();
    }
}
