namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface ILettersRepository
    {
        Task<List<char>> GetAllLetters();
        Task<bool> Add(char letter);
        Task<bool> Delete(char letter);
    }
}
