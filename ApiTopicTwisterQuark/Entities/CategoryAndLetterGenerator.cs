using ApiTopicTwisterQuark.Repositories;
using ApiTopicTwisterQuark.Repositories.Interfaces;

namespace ApiTopicTwisterQuark.Entities
{
    
    public class CategoryAndLetterGenerator
    {
        private ICategoryRepository categoriesRepository;
        private ILettersRepository lettersRepository;
        public CategoryAndLetterGenerator(ICategoryRepository categoriesRepository, ILettersRepository lettersRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.lettersRepository = lettersRepository;
        }

        public async Task<List<string> > GenerateThisNumberOfCategories(int numberOfCategoriesToReturn)
        {           
            List<string> auxCategories = new List<string>( await categoriesRepository.GetAllCategoriesNames() );
            Shuffle(auxCategories);            
            return auxCategories.Take(numberOfCategoriesToReturn).ToList();
        }
        public async Task<List<char>>  GenerateThisNumberOfLetters(int numberOfLettersToReturn)
        {
            List<char> auxLetters = new List<char>(await lettersRepository.GetAllLetters());
            Shuffle(auxLetters);

            return auxLetters.Take(numberOfLettersToReturn).ToList();
        }

        private void Shuffle<T>(List<T> inputList)
        {
            Random random = new Random();
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                int j = random.Next(i, inputList.Count);
                (inputList[i], inputList[j]) = (inputList[j], inputList[i]);
            }
        }
    }
}

