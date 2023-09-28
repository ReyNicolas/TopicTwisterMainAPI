using ApiTopicTwisterQuark.Entities;

namespace ApiTopicTwisterQuark.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<string>> GetAllCategoriesNames();
        Task<bool> Add(Category category);
        Task<Category> GetCategoryByName(string name);
        Task<bool> Update(Category category);
    }

}
