using NUnit.Framework;

namespace ApiTopicTwisterQuark.Entities.Tests
{
    public class TurnTest
    {
          private List<Category> categories;
        string playerID;
        string matchID;
        int roundID;
        int turnID;
        private char letter;
        private float time;
        private bool finish;
        private Turn turn;
        private int correctCount;

        [SetUp]
        public void Setup()
        {
            categories = new List<Category>() { new Category("Deportes", new List<string>() { "ARQUERIA" }) };
            playerID = "Player";
            matchID = "Match";
            roundID = 1;
            turnID = 1;
            letter = 'A';
            time = 60;
            finish = false;
            correctCount = 0;
            turn = new Turn(matchID,roundID, turnID, playerID, time,finish,correctCount);
        }

        [Test]
        public void SetAnswer()
        {
            var categoryName = categories.First().Name;
            var expectedWord = categories.First().words.First();
            List<Answer> answers = new List<Answer>()
            {
                new Answer(matchID,roundID,turnID,categoryName,expectedWord , letter)
            };
            turn.SetAnswers(answers);
            var result = turn.GetAnswerOfCategoryName(categoryName).word;
            
            Assert.AreEqual(expectedWord,result);
        }

        [Test]
        public void IsPlayer_PlayerIDInTurnIsEqual_ReturnTrue()
        {
            var playerIDToCheck = playerID;

            var result = turn.IsPlayer(playerIDToCheck);
            
            Assert.IsTrue(result);
        }
        [Test]
        public void SetOver_SetTurnFinishToTrue()
        {
            var category = categories.First();
            var categoryName = category.Name;
            var expectedWord = category.words.First();
            var answer = new Answer(matchID, roundID, turnID, categoryName, expectedWord, letter);
            answer.SetCategory(category);
            List<Answer> answers = new List<Answer>()
            {
                answer
            };
            turn.SetAnswers(answers);
            turn.SetOver();
            var result = turn.Finish;

            Assert.IsTrue(result);
        }

        [Test]
        public void SetOver_WhenOneAnswerCorrect_SetCorreCountInOne()
        {
            var category = categories.First();
            var categoryName = category.Name;
            var expectedWord = category.words.First();
            var answer = new Answer(matchID, roundID, turnID, categoryName, expectedWord, letter);
            answer.SetCategory(category);
            List<Answer> answers = new List<Answer>()
            {
                answer
            };
            var expectedResult = answers.Count(a => a.IsCorrect());
            turn.SetAnswers(answers);
            
            turn.SetOver();
            var result = turn.CorrectCount;

            Assert.AreEqual(expectedResult,result);
        }
    }
    
}
