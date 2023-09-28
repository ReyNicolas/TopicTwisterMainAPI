using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTopicTwisterQuark.Repositories;



public class PlayerRepository : IPlayerRepository
{
    private IPlayerService playerService;
    public PlayerRepository(IPlayerService playerService)
    {
        this.playerService = playerService;
    }
    
    
    public async Task<Player> Get(string playerID)
    {
        return await playerService.Get(playerID);
    }

    public async Task<List<Player>> GetPlayers()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Add(Player player)
    {
        return await playerService.Add(player);
    }

    public async Task<bool> Update(Player player)
    {
        return await playerService.Update(player);

    }

}
