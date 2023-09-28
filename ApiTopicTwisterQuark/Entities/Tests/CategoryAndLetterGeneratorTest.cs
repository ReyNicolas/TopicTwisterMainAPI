using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace ApiTopicTwisterQuark.Entities.Tests
{
   
    public class CategoryAndLetterGeneratorTest
    {
        private ICategoryRepository categoriesRepository;
        private ILettersRepository lettersRepository;

        [SetUp]
        public void Setup()
        {
            categoriesRepository = Substitute.For<ICategoryRepository>();
            lettersRepository = Substitute.For<ILettersRepository>();
        }
        [Test]
        public async Task GenerateThisNumberOfCategories_WhenNumberOfCategoriesIsZero_ReturnEmptyList()
        {
            List<string> categories = new List<string>() { "Animales", "Ciudades" };
            List<char> letters = new List<char>() { 'A', 'C' };
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            
            int numberOfCategoriesToReturn = 0;
            CategoryAndLetterGenerator generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            var result = await generator.GenerateThisNumberOfCategories(numberOfCategoriesToReturn);
            var resultCount = result.Count;

            Assert.AreEqual(numberOfCategoriesToReturn, resultCount);
        }
        [Test]
        public async Task GenerateThisNumberOfCategories_WhenNumberOfCategoriesIsOne_ReturnOneCategory()
        {
            List<string> categories = new List<string>() { "Animales", "Ciudades" };
            List<char> letters = new List<char>() { 'A', 'C' };
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            int numberOfCategoriesToReturn = 1;


            CategoryAndLetterGenerator generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            var result = await generator.GenerateThisNumberOfCategories(numberOfCategoriesToReturn);
            var resultCount = result.Count;

            Assert.AreEqual(numberOfCategoriesToReturn, resultCount);
            Assert.IsTrue(categories.Contains(result.First()));
        }
        [Test]
        public async Task GenerateThisNumberOfCategories_WhenNumberOfCategoriesIsThreeAndGeneratorHaveFive_ReturnThreeCategories()
        {
            List<string> categories = new List<string>() { "Animales", "Ciudades", "Futbol", "Television", "Musica" };
            List<string> categoriesToCheck = new List<string>() { categories[0], categories[2], categories[4] };
            List<char> letters = new List<char>() { 'A', 'C' };
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            int numberOfCategoriesToReturn = 3;


            CategoryAndLetterGenerator generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            var result = await generator.GenerateThisNumberOfCategories(numberOfCategoriesToReturn);
            var resultCount = result.Count;

            Assert.AreEqual(numberOfCategoriesToReturn, resultCount);
            Assert.IsTrue(categories.Contains(result[0]));
            Assert.IsTrue(categories.Contains(result[1]));
            Assert.IsTrue(categories.Contains(result[2]));
            Assert.IsTrue(categoriesToCheck.Any(cTC => result.Any(r => cTC == r)));
        }
        [Test]
        public async Task GenerateThisNumberOfLetters_WhenNumberOfLettersIsZero_ReturnEmptyList()
        {
            List<string> categories = new List<string>() { "Animales", "Ciudades" };
            List<char> letters = new List<char>() { 'A', 'C' };
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            int numberOfLettersToReturn = 0;

            CategoryAndLetterGenerator generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            var result =  await generator.GenerateThisNumberOfLetters(numberOfLettersToReturn);
            var resutlCount = result.Count;


            Assert.AreEqual(numberOfLettersToReturn, resutlCount);
        }

        [Test]
        public async Task GenerateThisNumberOfLetters_WhenNumberOfLettersIsOneAndGeneratorHaveTwo_ReturnOneLetter()
        {
            List<string> categories = new List<string>() { "Animales", "Ciudades" };
            List<char> letters = new List<char>() { 'A', 'C' };
            categoriesRepository.GetAllCategoriesNames().Returns(categories);
            lettersRepository.GetAllLetters().Returns(letters);
            int numberOfLettersToReturn = 1;

            CategoryAndLetterGenerator generator = new CategoryAndLetterGenerator(categoriesRepository, lettersRepository);
            var result = await generator.GenerateThisNumberOfLetters(numberOfLettersToReturn);
            var resultCount = result.Count;

            Assert.AreEqual(numberOfLettersToReturn, resultCount);
            Assert.IsTrue(letters.Contains(result.First()));
        }
    }
}
