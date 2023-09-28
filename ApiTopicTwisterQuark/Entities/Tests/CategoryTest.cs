using NUnit.Framework;

namespace ApiTopicTwisterQuark.Entities.Tests
{
    public class CategoryTest
    {
        private string name;
        private List<string> words;
        private List<char> letters;

        [SetUp]
        public void Setup()
        {
            name = "DEPORTES";
            words = new List<string>() { "FUTBOL", "BASQUET", "TENIS" };
            letters = new List<char>() { 'A', 'B'};
        }
        [Test]
        public void ContainsThisWordAndThatMustStartWithThisLetter_CategoryContainTheWordWithLetter_ReturnTrue()
        {
            var category = new Category(name, words);
            var wordToCheck = words[1];
            var letter = letters[1];

            var result = category.ContainsThisWordAndThatMustStartWithThisLetter(wordToCheck,letter);
            
            Assert.IsTrue(result);
        }
            
        [Test]
        public void  ContainsThisWordAndThatMustStartWithThisLetter_DoesntContainTheWordWithLetter_ReturnFalse()
        {
            var wordToCheck = "BOXEO";
            var category = new Category(name, words);
            var letter = wordToCheck.First();

            var result = category.ContainsThisWordAndThatMustStartWithThisLetter(wordToCheck,letter);

            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsThisWordAndThatMustStartWithThisLetter_ContainsWordButNotStartWithLetter_ReturnFalse()
        {
            var wordToCheck = words.First();
            var category = new Category(name, words);
            var letter = letters.First();

            var result = category.ContainsThisWordAndThatMustStartWithThisLetter(wordToCheck, letter);

            Assert.AreNotEqual(letter, wordToCheck.First());
            Assert.IsFalse(result);
        }

    }
}
