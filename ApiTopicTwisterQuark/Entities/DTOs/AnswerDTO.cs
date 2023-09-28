namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class AnswerDTO
    {
        public string CategoryName;
        public string Word;
        public char Letter;
        public bool IsCorrect;
        

        public AnswerDTO( string categoryName, string word, char letter, bool isCorrect)
        {
            CategoryName = categoryName;
            Letter = letter;
            Word = word;
            IsCorrect = isCorrect;
        }
    }
}