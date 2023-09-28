namespace ApiTopicTwisterQuark.Entities
{
    public class Turn 
    {
    
        public string MatchID { get; }
        public int RoundID { get; }
        public int ID { get; }
        public string PlayerID { get; }
        public float Time { get; set; }
        
        List<Answer> answers;
        private int correctCount;
        private bool finish;
        public int CorrectCount
        {
            get { return correctCount; }
        }
        public bool Finish
        {
            get { return finish; }
        }


        public Turn( string matchID,int roundID,int id, string playerID, float time, bool finish, int correctCount)
        {
            MatchID = matchID;
            RoundID = roundID;
            ID = id;
            PlayerID = playerID;
            Time = time;
            this.finish = finish;
            this.correctCount = correctCount;
        }

        public bool IsPlayer(string playerID)
        {
            return PlayerID == playerID;
        }

        public void SetAnswers(List <Answer> answers)
        {
            this.answers = answers;
           
        }

        public void SetOver()
        {
            CalculateCorrectAnswers();
            finish = true;
        }
        public Answer GetAnswerOfCategoryName(string categoryName)
        {
            return answers.Find(a => a.categoryName == categoryName);

        }
        private void CalculateCorrectAnswers()
        {
            correctCount = answers.Count(a => a.IsCorrect());
        }       
    }
}