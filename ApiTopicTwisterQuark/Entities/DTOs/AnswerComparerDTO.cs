namespace ApiTopicTwisterQuark.Entities.DTOs
{
    public class AnswerComparerDTO
    {
        public WordAndResultDTO PlayerWordResult;
        public WordAndResultDTO RivalWordResult;
        public string CategoryName;

        public AnswerComparerDTO(WordAndResultDTO playerWordResult, WordAndResultDTO rivalWordResult, string categoryName)
        {
            PlayerWordResult = playerWordResult;
            RivalWordResult = rivalWordResult;
            CategoryName = categoryName;
        }
    }
}