using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiTopicTwisterQuark.Repositories
{
    

    public class RoundRepository : IRoundRepository
    {
        private TopicTwisterContext context;

        public RoundRepository(TopicTwisterContext context)
        {
            this.context = context;
        }
        public async Task<Round> Get(string matchID, int roundID)
        {
            RoundEntity roundEntity = await context.Rounds.FirstAsync(r =>
                r.MatchID == matchID && r.ID == roundID);

            List<string> roundCategoriesNames = await context.RoundsCategories.Where(
                rc => rc.MatchID == matchID && rc.RoundID == roundID).Select(rc => rc.CategoryName).ToListAsync();

            Round round = new Round(roundEntity.MatchID, roundEntity.ID, roundCategoriesNames, roundEntity.Letter,
                roundEntity.WinnerID,
                roundEntity.Tie, roundEntity.InitialTimePerTurn);
            
            return round;
        }

        public async Task<List<Round>> GetRoundsFromMatchID(string matchID)
        {
            List<RoundEntity> roundEntities = await context.Rounds.Where(r => r.MatchID == matchID).ToListAsync();

            List<Round> rounds = new List<Round>();

            foreach (var roundEntity in roundEntities)
            {
                rounds.Add( await Get(roundEntity.MatchID,roundEntity.ID) );
            }

            return rounds;
        }   

        public async Task<bool> Add(Round round)
        {
            RoundEntity roundEntity = new RoundEntity();
            roundEntity.MatchID = round.MatchID;
            roundEntity.ID = round.ID;
            roundEntity.InitialTimePerTurn = round.InitialTimePerTurn;
            roundEntity.Letter = round.Letter;
            roundEntity.WinnerID = round.WinnerID;
            roundEntity.Tie = round.Tie;
            await context.Rounds.AddAsync(roundEntity);

            foreach (var categoryName in round.Categories)
            {
                RoundCategoryEntity roundCategoryEntity = new RoundCategoryEntity();
                roundCategoryEntity.CategoryName = categoryName;
                roundCategoryEntity.MatchID = round.MatchID;
                roundCategoryEntity.RoundID = round.ID;
                await context.RoundsCategories.AddAsync(roundCategoryEntity);
            }
            
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(Round round)
        {
            RoundEntity roundEntity = await context.Rounds.FirstAsync(r => r.MatchID == round.MatchID &&
                                                                           r.ID == round.ID);
            roundEntity.WinnerID = round.WinnerID;
            roundEntity.Tie = round.Tie;
           
            await context.SaveChangesAsync();

            return true;
        }

        public Task<bool> Delete(string matchID, int roundID)
        {
            throw new NotImplementedException();
        }
    }


}