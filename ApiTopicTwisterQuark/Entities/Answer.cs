namespace ApiTopicTwisterQuark.Entities
{
    [Serializable]
    public class Answer
    {
        public string MatchID { get; }
        public int RoundID { get; }
        public int TurnID { get; }
        public string categoryName;
        public string word;
        public char Letter { get; }
        
        private Category category;
        
        public Answer(string matchID, int roundID, int turnID, string categoryName, string word,char letter)
        {
            this.categoryName = categoryName;
            Letter = letter;
            this.word = word;
            MatchID = matchID;
            RoundID = roundID;
            TurnID = turnID;
        }

        public void SetCategory(Category category)
        {
            this.category = category;
        }

        public bool IsCorrect()
        {
           return category.ContainsThisWordAndThatMustStartWithThisLetter(word, Letter);
        }
    }
}