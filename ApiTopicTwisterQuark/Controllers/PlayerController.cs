using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using ApiTopicTwisterQuark.Services;
using ApiTopicTwisterQuark.Services.Interfaces;
using ApiTopicTwisterQuark.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiTopicTwisterQuark.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : Controller
{
    private GetMatchHistory getMatchHistory;
    private GetMatchesToPlayInfo getMatchesToPlayInfo;
    private GetPlayerStats getPlayerStats;
    private INotificationService notificationService;
    private IPlayerService playerService;

    public PlayerController(IMatchRepository matchRepository, IRoundRepository roundRepository, ITurnRepository turnRepository, IPlayerService playerService,INotificationService notificationService)
    {

        getMatchHistory = new GetMatchHistory(matchRepository, roundRepository);
        getMatchesToPlayInfo = new GetMatchesToPlayInfo(matchRepository, roundRepository, turnRepository);
        this.playerService = playerService;
        this.notificationService = notificationService;
        getPlayerStats = new GetPlayerStats(playerService);
    }
    [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResultDTO))]
        public async Task<ActionResult> RegisterPlayer(RegisterPlayerData registerPlayerData)
        {
            try
            {
                if (registerPlayerData == null)return BadRequest("Not information sent");
                var result = await playerService.Register(registerPlayerData);
                var resultJson = JsonConvert.SerializeObject(result);
                return Content(resultJson, "application/json");
            }
            catch (Exception e)
            {
                return StatusCode(404, $"Error: {e}");

            }
        }
        
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerDTO))]
    public async Task<ActionResult> CheckLogin(string playerID, string password)
    {
        try
        {
            if (playerID == null || password == null)return BadRequest("El campo no puede ser null");
            var result= await playerService.Login(playerID, password);
           var resultJson = JsonConvert.SerializeObject(result);
           return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de jugador no existe o el password no se encuentra: {e}");

        }
        
    }
    
    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MatchResultDTO>))]
    public async Task<ActionResult> GetMatchHistory(string playerID)
    {
        try
        {
            if (playerID == null)return BadRequest("El campo no puede ser null");
            var result= await getMatchHistory.Execute(playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de jugador no existe: {e}");
        }
        
    }
    
    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MatchInfoDTO>))]
    public async Task<ActionResult> GetMatchesToPlay(string playerID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            var result= await getMatchesToPlayInfo.Execute(playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al buscar partidas por jugador: {e}");
        }        
    }
    
    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MatchInfoDTO>))]
    public async Task<ActionResult> GetPlayerStats(string playerID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            var result= await getPlayerStats.Execute(playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al buscar stats de jugador: {e}");
        }        
    }
    
    
    
    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ForGameNotificationDTO>))]
    public async Task<ActionResult> GetPlayerForGameNotifications(string playerID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            var result= await notificationService.GetPlayerForGameNotification(playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al buscar notificaiones de jugador: {e}");
        }        
    }

    [HttpDelete("{playerID}/[action]/{notificationID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemovePlayerForGameNotification(string playerID,string notificationID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            await notificationService.RemovePlayerForGameNotification(playerID, notificationID);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al buscar notificaiones de jugador: {e}");
        }
        return StatusCode(200);
    }

    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlayerRematchNotificationDTO>))]
    public async Task<ActionResult> GetPlayerRematchNotifications(string playerID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            var result= await notificationService.GetPlayerRematchNotifications(playerID);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al buscar notificaiones de jugador: {e}");
        }        
    }
    [HttpPost("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SendRematchNotification(string playerID,RematchNotificationToSendDTO rematchNotification)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            await notificationService.AddPlayerRematchNotification(playerID,rematchNotification.receiverPlayerID);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al enviar notificacion: {e}");
        }
        return StatusCode(200);
    }
    [HttpDelete("{playerID}/[action]/{rivalID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemovePlayerRematchNotification(string playerID, string rivalID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            await notificationService.RemovePlayerRematchNotification(playerID, rivalID);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"Error al enviar notificacion: {e}");
        }
        return StatusCode(200);
    }
    
    [HttpGet("{playerID}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> FindNewRival(string playerID)
    {
        try
        {
            if (playerID == null) return BadRequest("El campo no puede ser null");
            var result= await playerService.GetRivalForPlayer(playerID);
           return Content(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, $"El ID de jugador no existe: {e}");
        }
        
    }

}