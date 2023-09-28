using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiTopicTwisterQuark.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoundController : Controller
{
    private EndRound endRound;

    public RoundController(IRoundRepository roundRepository, ITurnRepository turnRepository,
        IAnswerRepository answerRepository,ICategoryRepository categoriesRepository)

    {
        endRound = new EndRound( roundRepository, turnRepository, answerRepository,categoriesRepository);
    }
    
    
    [HttpGet("{matchID}/{roundID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> CheckIfOver(string matchID,int roundID)
    {
        try
        {
            if (matchID == null || roundID == null)return BadRequest("El ID no puede ser null");
            bool result = await endRound.IsActualRoundOver(matchID,roundID);
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada: {e}");
        }
        
    }
    
    [HttpGet("{matchID}/{roundID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> EndRound(string matchID,int roundID)
    {
        try
        {
            if (matchID == null || roundID == null)return BadRequest("El ID no puede ser null");
            await endRound.EndThisRound(matchID,roundID);
            return Ok();

        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada: {e}");
        }
        
    }
    [HttpGet("{matchID}/{roundID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoundResultDTO))]
    public async Task<IActionResult> GetRoundResult(string matchID,int roundID,string playerID)
    {
        try
        {
            if (matchID == null || roundID == null || playerID == null)return BadRequest("El ID no puede ser null");
            var result =  await endRound.ReturnRoundResultForPlayer(matchID,roundID,playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada o el ID de jugador no está vinculado con la partida: {e}");
        }
        
    }
    
    
}

