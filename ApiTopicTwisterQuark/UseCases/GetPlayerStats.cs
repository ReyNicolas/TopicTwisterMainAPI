using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Services.Interfaces;

namespace ApiTopicTwisterQuark.UseCases;

public class GetPlayerStats
{
    private IPlayerService playerService;
    public GetPlayerStats(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    public async Task<PlayerStats> Execute(string playerID)
    {
        var player = await playerService.Get(playerID);
        return new PlayerStats(player.VictoryPoints, 0);
    }
    
}

