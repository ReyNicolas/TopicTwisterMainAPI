using ApiTopicTwisterQuark.Data;
using ApiTopicTwisterQuark.Entities;
using ApiTopicTwisterQuark.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ApiTopicTwisterQuark.Repositories;



public class AnswerRepository :IAnswerRepository 
{
    private TopicTwisterContext context;
    public AnswerRepository(TopicTwisterContext context)
    {
        this.context = context;
    }
    
    public async Task<Answer> Get(string matchID, int roundID, int turnID, string categoryName)
    {
        AnswerEntity answerEntity =
            await context.Answers.FirstAsync(a =>
                a.MatchID == matchID && a.RoundID == roundID
                                     && a.TurnID == turnID &&
                                     a.CategoryName == categoryName);
        Answer answer = new Answer(answerEntity.MatchID, answerEntity.RoundID, answerEntity.TurnID,
            answerEntity.CategoryName, answerEntity.Word, answerEntity.Letter);
        return answer;
    }

    public async Task<List<Answer>> GetAnswersFromTurn(string matchID, int roundID, int turnID)
    {
        List<AnswerEntity> answerEntities = await context.Answers.Where(a => a.MatchID == matchID && a.RoundID == roundID && a.TurnID == turnID).ToListAsync();

        List<Answer> answers = new List<Answer>();

        foreach (var answerEntity in answerEntities)
        {
            answers.Add( await Get(answerEntity.MatchID,answerEntity.RoundID,answerEntity.TurnID, answerEntity.CategoryName) );
        }

        return answers;
    }

    public async Task<bool> Add(Answer answer)
    {
        AnswerEntity answerEntity = new AnswerEntity();
        answerEntity.MatchID = answer.MatchID;
        answerEntity.RoundID = answer.RoundID;
        answerEntity.TurnID = answer.TurnID;
        answerEntity.CategoryName = answer.categoryName;
        answerEntity.Word = answer.word;
        answerEntity.Letter = answer.Letter;
        await context.AddAsync(answerEntity);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(Answer answer)
    {
        AnswerEntity answerEntity = await context.Answers.FirstAsync(a =>
                a.MatchID == answer.MatchID && a.RoundID == answer.RoundID
                                     && a.TurnID == answer.TurnID &&
                                     a.CategoryName == answer.categoryName);
        answerEntity.Word = answer.word;
        await context.SaveChangesAsync();
        return true;
    }

    
       
}