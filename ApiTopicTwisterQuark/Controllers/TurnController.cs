using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiTopicTwisterQuark.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TurnController : Controller
{
    private StartTurn startTurn;
    private EndTurn endTurn;
    public TurnController(IMatchRepository matchRepository, IRoundRepository roundRepository,
            ITurnRepository turnRepository, ICategoryRepository categoryRepository,
            IAnswerRepository answerRepository)
   {
        startTurn = new StartTurn(matchRepository, roundRepository, turnRepository);
        endTurn = new EndTurn(categoryRepository,turnRepository,answerRepository);
   }
   
    [HttpGet("{matchID}/{roundID}/{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StartTurnDTO))]
    public async Task<ActionResult> GetTurn(string matchID,int roundID,string playerID)
    {
        try
        {
            if (matchID == null || roundID == null || playerID == null)return BadRequest("El ID no puede ser null");
            StartTurnDTO result = await startTurn.GetStartTurnData(matchID,roundID,playerID) ;

            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada o el ID de jugador no está vinculado con la partida: {e}");
        }       
       
    }

    [HttpPost("{matchID}/{roundID}/{turnID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK) ]
    public async Task<IActionResult> SetAnswer(string matchID,int roundID,int turnID, AnswerPost answerPost)
    {
        try
        {
            if (matchID == null || roundID == null || turnID == null)return BadRequest("El ID no puede ser null");
            await endTurn.SetAnswer(matchID,roundID,turnID, answerPost.Word, answerPost.CategoryName) ;
            return new OkResult(); 
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada o el ID del turno no ha sido encontrado: {e}");
        }
    }
    
    [HttpGet("{matchID}/{roundID}/{turnID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK) ]
    public async Task<IActionResult> EndTurn(string matchID,int roundID,int turnID, float timeLeft)
    {
        try
        {
            if (matchID == null || roundID == null || turnID == null || timeLeft == null)return BadRequest("El ID no puede ser null o el tiempo no puede ser null");
            DataApplication dataApplication = new DataApplication(matchID, roundID, turnID, 0, "None");
            await endTurn.EndThisTurn(dataApplication,timeLeft);
            return new OkResult();
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada o el ID del turno no ha sido encontrado: {e}");
        }
        
    }
    
    [HttpGet("{matchID}/{roundID}/{turnID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TurnResultDTO))]
    public async Task<ActionResult> GetTurnResult(string matchID,int roundID,int turnID)
    {
        try
        {
            if (matchID == null || roundID == null || turnID == null)return BadRequest("El ID no puede ser null");
            TurnResultDTO result = await endTurn.GetTurnResult(matchID,roundID,turnID) ;

            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de la ronda no ha sido encontrada o el ID del turno no ha sido encontrado: {e}");
        }
        
       
    }
    
}

