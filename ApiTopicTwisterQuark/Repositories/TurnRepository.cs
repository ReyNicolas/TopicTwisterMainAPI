using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTopicTwisterQuark.Repositories;



public class TurnRepository : ITurnRepository
{
    private TopicTwisterContext context;

    public TurnRepository(TopicTwisterContext context)
    {
        this.context = context;
    }

    public async Task<Turn> Get(string matchID, int roundID, int turnID)
    {
        TurnEntity turnEntity = await context.Turns.FirstOrDefaultAsync(t => t.MatchID == matchID &&
                                                                             t.RoundID == roundID && t.ID == turnID);
        if (turnEntity == null) return null;
        Turn turn = new Turn(turnEntity.MatchID, turnEntity.RoundID, turnEntity.ID, turnEntity.PlayerID,
            turnEntity.Time, turnEntity.Finish, turnEntity.CorrectCount);

        return  turn;
    }

    public async Task<List<Turn>> GetTurnsFromMatchIDAndRoundID(string matchID, int roundID)
    {
        List<TurnEntity> turnEntities = await context.Turns.Where(t => t.MatchID == matchID && t.RoundID == roundID).ToListAsync();

        List<Turn> turns = new List<Turn>();

        foreach (var turnEntity in turnEntities)
        {
            turns.Add( await Get(turnEntity.MatchID,turnEntity.RoundID,turnEntity.ID) );
        }

        return turns;
    }

    public async Task<bool> Add(Turn turn)
    {
        TurnEntity turnEntity = new TurnEntity();
        turnEntity.MatchID = turn.MatchID;
        turnEntity.RoundID = turn.RoundID;
        turnEntity.ID = turn.ID;
        turnEntity.PlayerID = turn.PlayerID;
        turnEntity.Time = turn.Time;
        turnEntity.CorrectCount = turn.CorrectCount;
        turnEntity.Finish = turn.Finish;


        await context.Turns.AddAsync(turnEntity);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(Turn turn)
    {
        TurnEntity turnEntity = await context.Turns.FirstAsync(t => t.MatchID == turn.MatchID &&
                                                                    t.RoundID == turn.RoundID && t.ID == turn.ID);

        turnEntity.Time = turn.Time;
        turnEntity.CorrectCount = turn.CorrectCount;
        turnEntity.Finish = turn.Finish;
   
        await context.SaveChangesAsync();

        return true;
    }

    public Task<bool> Delete(string matchID, int roundID, int turnID)
    {
        throw new NotImplementedException();
    }
}