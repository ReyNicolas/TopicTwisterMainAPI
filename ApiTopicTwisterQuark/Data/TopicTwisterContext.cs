using System.ComponentModel.DataAnnotations;
using ApiTopicTwisterQuark.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiTopicTwisterQuark.Data;

public class TopicTwisterContext : DbContext
{
    public TopicTwisterContext(DbContextOptions<TopicTwisterContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoundEntity>()
            .HasKey(r => new { r.MatchID, r.ID });

        modelBuilder.Entity<RoundCategoryEntity>()
            .HasKey(rc => new { rc.MatchID, rc.RoundID, rc.CategoryName  }); // roundca
        
        modelBuilder.Entity<CategoryWordEntity>()
            .HasKey(cw => new {  cw.CategoryName, cw.Word }); //caword
        
        modelBuilder.Entity<MatchPlayerEntity>()
            .HasKey(mp => new { mp.MatchID, mp.PlayerID }); //matchpla
        
        modelBuilder.Entity<AnswerEntity>()
            .HasKey(a => new { a.MatchID, a.RoundID, a.TurnID, a.CategoryName }); //Answer
        
        modelBuilder.Entity<TurnEntity>()
            .HasKey(t => new { t.MatchID, t.RoundID, t.ID }); //Turn
       
        modelBuilder.Entity<ForGameNotificationEntity>()
            .HasKey(fg=>new {fg.PlayerID, fg.NotificationID});
        
         modelBuilder.Entity<RematchNotificationEntity>()
            .HasKey(n=>new {n.RivalID, n.NotificationID, n.PlayerID});
       
        modelBuilder.Entity<RoundCategoryEntity>()
            .HasOne(rc => rc.Round)
            .WithMany(r => r.RoundCategories)
            .HasForeignKey(rc => new {rc.MatchID, rc.RoundID }); // RoundCategories-Round           
        
        modelBuilder.Entity<RoundCategoryEntity>()
            .HasOne(rc => rc.Category)
            .WithMany(c => c.RoundsCategories)
            .HasForeignKey(rc => rc.CategoryName); // RoundCategories-Category
        
        modelBuilder.Entity<CategoryWordEntity>()
            .HasOne(cw => cw.Category)
            .WithMany(c => c.CategoriesWords)
            .HasForeignKey(rc =>  rc.CategoryName); // Categoria - word
        
        modelBuilder.Entity<MatchPlayerEntity>()
            .HasOne(mp => mp.Match)
            .WithMany(c => c.MatchesPlayers)
            .HasForeignKey(rc => rc.MatchID); // Matchplayers - match
        
        modelBuilder.Entity<MatchPlayerEntity>()
            .HasOne(mp => mp.Player)
            .WithMany(c => c.MatchsPlayersEntities)
            .HasForeignKey(rc => rc.PlayerID); // Matchplayers - player

    }
    
    public DbSet<AnswerEntity> Answers { get; set; }
    public DbSet<CategoryWordEntity> CategoriesWords { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<LetterEntity> Letters { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<MatchPlayerEntity> MatchesPlayers { get; set; }
    public DbSet<RoundCategoryEntity> RoundsCategories { get; set; }
    public DbSet<RoundEntity> Rounds { get; set; }
    public DbSet<TurnEntity> Turns { get; set; }
    public DbSet<RematchNotificationEntity> RematchesNotifications { get; set; }
    public DbSet<ForGameNotificationEntity> ForGamesNotifications  { get; set; }
}

public class AnswerEntity
{
    public string MatchID { get; set; }
    public int RoundID { get; set; }
    public int TurnID { get; set; }
    public string CategoryName { get; set; }
    public string Word{ get; set; }
    public char Letter{ get; set; }
   
}

public class CategoryWordEntity
{
    public string CategoryName { get; set; }
    public string Word{ get; set; }
    public CategoryEntity Category { get; set; }
}

public class CategoryEntity
{
    [Key]
    public string Name { get; set; }
    public ICollection<RoundCategoryEntity> RoundsCategories { get; set; }
    public ICollection<CategoryWordEntity> CategoriesWords { get; set; }
}

public class RoundCategoryEntity
{
    public string MatchID { get; set; }
    public int RoundID { get; set; }
    public string CategoryName { get; set; }
    
    public RoundEntity Round { get; set; } 
    public CategoryEntity Category { get; set; }
}

public class LetterEntity
{
    [Key]
    public char Value { get; set; }
}

public class PlayerEntity
{
    public string ID { get; set; }
    public int VictoryPoints { get; set; }
    public string Password { get; set; }
    public ICollection<MatchPlayerEntity> MatchsPlayersEntities { get; set; }
}



public class ForGameNotificationEntity
{
    public string PlayerID { get; set; }
    public string NotificationID { get; set; }
}
public class RematchNotificationEntity
{
    public string RivalID { get; set; }
    public string NotificationID { get; set; }
    public string PlayerID { get; set; }

}

public class MatchPlayerEntity
{
    public string MatchID { get; set; }
    public string PlayerID { get; set; }
    public MatchEntity Match { get; set; }
    public PlayerEntity Player { get; set; }

}
public class MatchEntity
{
    public string ID { get; set; }
    public string WinnerID { get; set; }
    public bool Tie{ get; set; }
    public  ICollection<MatchPlayerEntity> MatchesPlayers { get; set; }
}
public class RoundEntity
{
    public string MatchID { get; set; }
    public int ID { get; set; }
    public float InitialTimePerTurn { get; set; }
    public char Letter { get; set; }
    public string WinnerID { get; set; }
    public bool Tie{ get; set; }
    public ICollection<RoundCategoryEntity> RoundCategories { get; set; }
}


public class TurnEntity
{
    public string MatchID { get; set; }
    public int RoundID { get; set; }
    public int ID { get; set; }
    public string PlayerID { get; set; }
    public float Time { get; set; }
    public int CorrectCount { get; set; }
    public bool Finish{ get; set; }
}