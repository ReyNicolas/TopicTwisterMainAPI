using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Services.Interfaces;

namespace ApiTopicTwisterQuark.UseCases;

public class FindNewRivalForPlayer
{
    private IPlayerService playerService;

    public FindNewRivalForPlayer(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    public async Task<string> Execute(string playerID)
    {
        return await playerService.GetRivalForPlayer(playerID);
    }

  
}