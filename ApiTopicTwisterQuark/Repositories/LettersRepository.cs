using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiTopicTwisterQuark.Repositories;


public class LettersRepository :   ILettersRepository
{
    private TopicTwisterContext context;

    public LettersRepository(TopicTwisterContext context)
    {
        this.context = context;
    }

    
    public async Task<List<char>> GetAllLetters()
    {
        return await context.Letters.Select(l=>l.Value).ToListAsync();
    }

    public async Task<bool> Add(char letter)
    {
        var letterEntity = new LetterEntity();
        letterEntity.Value = letter;
        await context.Letters.AddAsync(letterEntity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(char letter)
    {
        var letterToRemove = await context.Letters.FirstAsync(l => l.Value == letter);
        context.Letters.Remove(letterToRemove);
        await context.SaveChangesAsync();
        return true;
    }


}

