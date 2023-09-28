using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface IRoundRepository
    {
        Task<Round> Get(string matchID, int roundID);

        Task<List<Round>> GetRoundsFromMatchID(string matchID);
        Task<bool> Add(Round round);
        Task<bool> Update(Round round);
        Task<bool> Delete(string matchID, int roundID);
    }
}
