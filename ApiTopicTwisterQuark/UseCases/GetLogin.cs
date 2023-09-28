using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;

using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Services.Interfaces;

namespace ApiTopicTwisterQuark.UseCases;

public class GetLogin
{
    private IPlayerService playerService;

    public GetLogin(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    public async Task<LoginResultDTO> Execute(string playerID,string password)
    {
       return await playerService.Login(playerID, password);
    }
}