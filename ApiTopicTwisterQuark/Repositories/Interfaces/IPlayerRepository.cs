using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> Get(string playerID);

        Task<bool> Add(Player player);
        Task<bool> Update(Player player);
    }
}
