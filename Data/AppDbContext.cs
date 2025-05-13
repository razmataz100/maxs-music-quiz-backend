using MaxsMusicQuiz.Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaxsMusicQuiz.Backend.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<QuizGame> QuizGames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameHistory>()
                .HasOne(gh => gh.QuizGame)
                .WithMany(qg => qg.GameHistories)
                .HasForeignKey(gh => gh.QuizGameId);

            modelBuilder.Entity<GameHistory>()
                .HasOne(gh => gh.User)
                .WithMany(u => u.GameHistories)
                .HasForeignKey(gh => gh.UserId);
        }
    }
}