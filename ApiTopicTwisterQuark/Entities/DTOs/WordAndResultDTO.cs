namespace ApiTopicTwisterQuark.Entities.DTOs;

public class WordAndResultDTO
{
    public string Word;
    public bool IsCorrect;

    public WordAndResultDTO(string word, bool isCorrect)
    {
        Word = word;
        IsCorrect = isCorrect;
    }
}