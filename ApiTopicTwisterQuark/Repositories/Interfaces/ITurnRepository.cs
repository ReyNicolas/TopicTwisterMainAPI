using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface ITurnRepository
    {
        Task<Turn> Get(string matchID, int roundID, int turnID);

        Task<List<Turn>> GetTurnsFromMatchIDAndRoundID(string matchID, int roundID);
        Task<bool> Add(Turn turn);
        Task<bool> Update(Turn turn);
        Task<bool> Delete(string matchID, int roundID, int turnID);
    }
}
