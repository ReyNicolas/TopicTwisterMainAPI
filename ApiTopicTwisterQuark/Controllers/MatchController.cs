using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ApiTopicTwisterQuark.Utils;

namespace ApiTopicTwisterQuark.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : Controller
{
    private CreateMatch createMatch;
    private EndMatch endMatch;
    private CheckTurnToPlay checkTurnToPlay;
    private SendEndMatchNotificationsToPlayers sendEndMatchNotificationsToPlayers;
    private SendMatchChangeNotificationsToPlayers SendMatchChangeNotificationsToPlayers;
    private AllForGameNotificationsIDs allForGameNotificationsIDs;

    public MatchController(IMatchRepository matchRepository, IRoundRepository roundRepository, ITurnRepository turnRepository, IPlayerRepository playerRepository, IAnswerRepository answerRepository, ICategoryRepository categoryRepository, ILettersRepository lettersRepository, INotificationService notificationService)
    {
        allForGameNotificationsIDs = new AllForGameNotificationsIDs();
        CategoryAndLetterGenerator categoryAndLetterGenerator = new CategoryAndLetterGenerator(categoryRepository, lettersRepository);
        createMatch = new CreateMatch(matchRepository, roundRepository, turnRepository, answerRepository, categoryAndLetterGenerator);
        endMatch = new EndMatch(matchRepository, roundRepository, playerRepository, notificationService);
        checkTurnToPlay = new CheckTurnToPlay(matchRepository, roundRepository, turnRepository);
        sendEndMatchNotificationsToPlayers = new SendEndMatchNotificationsToPlayers(matchRepository, notificationService, allForGameNotificationsIDs);
        SendMatchChangeNotificationsToPlayers = new SendMatchChangeNotificationsToPlayers(matchRepository, notificationService, allForGameNotificationsIDs);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> CreateMatch(MatchConfiguration matchConfiguration)
    {
        try
        {
            if (matchConfiguration == null) return BadRequest("La configuracion no puede ser null");
            string result = await createMatch.CreateMatchWithThisPlayersIDsAndNumberOfRounds(matchConfiguration.PlayersIDs, matchConfiguration.NumberOfRounds, matchConfiguration.CategoriesPerRound, matchConfiguration.TimePerTurn);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"No se creo la partida correctamente: {e}");
        }

    }

    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> CheckIfOver(string matchID)
    {
        try
        {
            if (matchID == null) return BadRequest("El ID no puede ser null");
            bool result = await endMatch.IsEndOfMatch(matchID);
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado: {e}");
        }
    }

    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> EndMatch(string matchID)
    {
        try
        {
            if (matchID == null) return BadRequest("El ID no puede ser null");
            await endMatch.EndThisMatch(matchID);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado: {e}");
        }

    }

    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendEndMatchNotificationToPlayers(string matchID)
    {
        try
        {
            if (matchID == null) return BadRequest("El ID no puede ser null");
            await sendEndMatchNotificationsToPlayers.Execute(matchID);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado: {e}");
        }
    }

    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMatchChangeNotificationToPlayers(string matchID)
    {
        try
        {
            if (matchID == null) return BadRequest("El ID no puede ser null");
            await SendMatchChangeNotificationsToPlayers.Execute(matchID);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado: {e}");
        }
    }


    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchResultDTO))]
    public async Task<ActionResult> GetResult(string matchID, string playerID)
    {
        try
        {
            if (matchID == null || playerID == null) return BadRequest("El ID no puede ser null");
            var result = await endMatch.ReturnMatchResultForPlayer(matchID, playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de jugador no está vinculado con la partida: {e}");
        }

    }

    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> IsThisPlayerTurn(string matchID, string playerID)
    {
        try
        {
            if (matchID == null || playerID == null) return BadRequest("El ID no puede ser null");
            bool result = await checkTurnToPlay.IsPlayerTurn(matchID, playerID);
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado o el ID de jugador no está vinculado con la partida: {e}");
        }

    }
    [HttpGet("{matchID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataApplication))]
    public async Task<ActionResult> GetDataToStartTurn(string matchID)
    {
        try
        {
            if (matchID == null) return BadRequest("El ID no puede ser null");
            var result = await checkTurnToPlay.GetDataToStartTurn(matchID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de la partida no ha sido encontrado: {e}");
        }

    }



}
