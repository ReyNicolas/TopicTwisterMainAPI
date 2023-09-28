using System.Diagnostics;
using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiTopicTwisterQuark.Repositories;


public class CategoryRepository : ICategoryRepository
{
    private TopicTwisterContext context;

    public CategoryRepository(TopicTwisterContext context)
    {
        this.context = context;      
    }
           
    
    public async Task<List<string>> GetAllCategoriesNames()
    {
      return await context.Categories.Select(c => c.Name).ToListAsync();
    }

    public async Task<bool> Add(Category category)
    {
        CategoryEntity categoryEntity = new CategoryEntity { Name = category.Name };
        await context.Categories.AddAsync(categoryEntity);
        await context.SaveChangesAsync();

        foreach (string word in category.words)
        {
            CategoryWordEntity categoryWordEntity = new CategoryWordEntity
            {
                CategoryName = category.Name,
                Word = word
            };
            await context.CategoriesWords.AddAsync(categoryWordEntity);
        }

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<Category> GetCategoryByName(string name)
    {
        List<string> words = await context.CategoriesWords.Where(
            cw => cw.CategoryName == name).Select(cw => cw.Word).ToListAsync();
        Category category = new Category(name, words);
        return category;
    }

    public Task<bool> Update(Category category)
    {
        throw new NotImplementedException();
    }
    

    
}
