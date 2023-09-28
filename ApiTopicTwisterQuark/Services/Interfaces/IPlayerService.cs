using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;

namespace ApiTopicTwisterQuark.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<Player> Get(string playerID);
        Task<bool> Add(Player player);
        Task<bool> Update(Player player);
        Task<LoginResultDTO> Login(string playerID, string passwordID);
        Task<LoginResultDTO> Register(RegisterPlayerData registPlayerData);

        Task<string> GetRivalForPlayer(string playerID);


    }
}
