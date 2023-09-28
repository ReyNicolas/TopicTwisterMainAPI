using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Entities.DTOs;
using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace ApiTopicTwisterQuark.UseCases.Tests
{
    public class EndTurnTest
    {
        private ICategoryRepository categoryRepository;
        private ITurnRepository turnRepository;
        private IAnswerRepository answerRepository;
        private EndTurn endTurn;
        private List<string> categoriesName;
        private List<Category> categories;
        private List<char> letters;
        private CategoryAndLetterGenerator generator;
        string matchID;
        int roundID;
        int turnID;
        string playerID;
        float startTime;
        float endTime;
        DataApplication dataApplication;

        [SetUp]
        public void Setup()
        {
             categoryRepository = Substitute.For<ICategoryRepository>();
             turnRepository = Substitute.For<ITurnRepository>();
             answerRepository = Substitute.For<IAnswerRepository>();
             endTurn = new EndTurn(categoryRepository,turnRepository,answerRepository);
             categoriesName = new List<string>()
                 { "Paises", "Ciudades", "Deportes", "Musica", "Arte", "Ciencia", "Tecnologia", "Naturaleza" };
             categories = new List<Category>()
             {
                 new Category(categoriesName[0], new List<string>() { "ARGENTINA" }),
                 new Category(categoriesName[1], new List<string>() { "ATENAS" }),
                 new Category(categoriesName[2], new List<string>() { "ARQUERIA" }),
             };
             letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F' };
             ICategoryRepository categoriesRepository = Substitute.For<ICategoryRepository>();
             ILettersRepository lettersRepository = Substitute.For<ILettersRepository>();
             categoriesRepository.GetAllCategoriesNames().Returns(categoriesName);
             lettersRepository.GetAllLetters().Returns(letters);
              generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
             matchID = "Match";
             roundID = 1;
             turnID = 1;
             playerID = "Player";
            startTime = 60;
            endTime = 30;
            dataApplication =
               new DataApplication(matchID, roundID, turnID, 3, playerID);
        }

        [Test]
        public async Task SetAnswer_UpdateAnswer()
        {
            //Arrange
            Category category;
            string word;
            Answer answer;
            SetWordToAnswer(out category, out word, out answer);

            //Act
            await endTurn.SetAnswer(matchID, roundID, turnID, word, category.Name);

            //Assert
            await answerRepository.Received().Update(answer);
            Assert.AreEqual(word, answer.word);
        }


        [Test]
        public async Task EndThisTurn_ShouldChangeTurnFinishToTrue()
        {
            //Arrange
            Turn turn = TurnNotFinished();

            //Act
            await endTurn.EndThisTurn(dataApplication, 40);
            var result = turn.Finish;

            //Assert
            Assert.IsTrue(result);

        }


        [Test]
        public async Task EndThisTurn_ShouldChangeTurnTime()
        {
            var turn = TurnNotFinished();

            await endTurn.EndThisTurn(dataApplication,endTime);
            var result = turn.Time;
            
            Assert.AreEqual(endTime, result);

        }
        [Test]
        public async Task EndThisTurn_ShouldSetAnswersAndCorrectCount()
        {
            //Arrange
            int expectedCount;
            Turn turn;
            TurnWithExpectedCorrectAnswers(out expectedCount, out turn);

            //Act
            await endTurn.EndThisTurn(dataApplication, endTime);
            var result = turn.CorrectCount;

            //Assert
            Assert.AreEqual(expectedCount, result);
        }


        [Test]
        public async Task EndThisTurn_ShouldUpdateMatch()
        {
            //Arrange
            var turn = TurnNotFinished();
           
            //Act
            await endTurn.EndThisTurn(dataApplication,endTime);
           
            //Assert
           await turnRepository.Received().Update(turn);
        }


        private void SetWordToAnswer(out Category category, out string word, out Answer answer)
        {
            category = new Category("DEPORTES", new List<string>() { "FUTBOL" });
            word = "FUTBOL";
            var letter = 'B';
            answer = new Answer(matchID, roundID, turnID, category.Name, "", letter);
            categoryRepository.GetCategoryByName(category.Name).Returns(category);
            answerRepository.Get(matchID, roundID, turnID, answer.categoryName).Returns(answer);
        }
        private Turn TurnNotFinished()
        {
            var category = new Category("CATEGORY", new List<string>() { "ANY" });
            Turn turn = new Turn(matchID, roundID, turnID, playerID, 60, false, 0);
            List<Answer> answers = categoriesName.Select(cat =>
                new Answer(matchID, roundID, turnID, cat, "", letters.First())).ToList();
            turnRepository.Get(matchID, roundID, turnID).Returns(turn);
            categoryRepository.GetCategoryByName(Arg.Any<string>()).Returns(category);
            answerRepository.GetAnswersFromTurn(matchID, roundID, turnID).Returns(answers);
            Assert.IsFalse(turn.Finish);
            return turn;
        }
        private void TurnWithExpectedCorrectAnswers(out int expectedCount, out Turn turn)
        {
            expectedCount = 2;
            turn = new Turn(matchID, roundID, turnID, playerID, startTime, false, 0);
            List<Answer> answers = new List<Answer>()
            {
                new Answer(matchID, roundID, turnID,categoriesName[0], "ARGENTINA",letters.First()),
                new Answer(matchID, roundID, turnID,categoriesName[1], "",letters.First()),
                new Answer(matchID, roundID, turnID,categoriesName[2], "ARQUERIA",letters.First())
            };
            turnRepository.Get(matchID, roundID, turnID).Returns(turn);
            answerRepository.GetAnswersFromTurn(matchID, roundID, turnID).Returns(answers);
            categoryRepository.GetCategoryByName(categories[0].Name).Returns(categories[0]);
            categoryRepository.GetCategoryByName(categories[1].Name).Returns(categories[1]);
            categoryRepository.GetCategoryByName(categories[2].Name).Returns(categories[2]);
        }
    }
}
