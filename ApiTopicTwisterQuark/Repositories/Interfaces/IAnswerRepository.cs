using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> Get(string matchID, int roundID, int turnID, string categoryName);

        Task<List<Answer>> GetAnswersFromTurn(string matchID, int roundID, int turnID);
        Task<bool> Add(Answer answer);
        Task<bool> Update(Answer answer);
    }
}
