using System.Text;
using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiTopicTwisterQuark.Services;



public class PlayerServiceKotlin:IPlayerService
{
    private HttpClient client;
    private string url;
    private TopicTwisterContext context;
    public PlayerServiceKotlin(TopicTwisterContext context)
    {
        this.context = context;
        client = new HttpClient();
        url = "https://topictwister-player-api.onrender.com/player";
    }
    
    public async Task<Player> Get(string playerID)
    {
        var response = await client.GetAsync($"{url}/{playerID}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Player>(content);
        }
        else
        {
            throw new Exception(response.RequestMessage?.ToString());
        }
    }

    public async Task<bool> Add(Player player)
    {
        string data = JsonConvert.SerializeObject(player);
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url,content);
        if (response.IsSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(jsonResponse);   
        }
        throw new Exception(response.RequestMessage?.ToString());
    }


    public async Task<bool> Update(Player player)
    {
        string data =  JsonConvert.SerializeObject(new PlayerData(player.ID,player.Password,player.VictoryPoints));
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = await client.PutAsync(url,content);
        if (response.IsSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
            return true;
        }
        throw new Exception(response.RequestMessage?.ToString());
        
    }

    public async Task<LoginResultDTO> Login(string playerID, string passwordID)
    {
        var response = await client.GetAsync($"{url}/login/{playerID}/{passwordID}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResultDTO>(content);
        }
        else
        {
            throw new Exception(response.RequestMessage?.ToString());
        }
    }

    public async Task<LoginResultDTO> Register(RegisterPlayerData registPlayerData)
    {
        string data =  JsonConvert.SerializeObject(registPlayerData);
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{url}",content);
        if (response.IsSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResultDTO>(jsonResponse);   
        }
        throw new Exception(response.RequestMessage?.ToString());

    }

    public async Task<string> GetRivalForPlayer(string playerID)
    {
        var response = await client.GetAsync($"{url}/getRivalForPlayer/{playerID}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        else
        {
            throw new Exception(response.RequestMessage?.ToString());
        }
    }

    
}
