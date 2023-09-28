using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetMatches();
        Task<Match> Get(string matchID);
        Task<string> GenerateNewMatchID();
        Task<bool> Add(Match match);
        Task<bool> Update(Match match);
        Task<bool> Delete(string matchID);
    }
}
