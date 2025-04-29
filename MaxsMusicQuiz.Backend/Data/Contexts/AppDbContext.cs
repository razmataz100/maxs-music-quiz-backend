using MaxsMusicQuiz.Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaxsMusicQuiz.Backend.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<QuizGame> QuizGames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuizGameUser> QuizGameUsers { get; set; } // Ensure this is added

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuizGameUser>().ToTable("QuizGameUsers");

            modelBuilder.Entity<QuizGameUser>()
                .HasKey(qgu => new { qgu.QuizGameId, qgu.UserId });

            modelBuilder.Entity<QuizGameUser>()
                .HasOne(qgu => qgu.QuizGame)
                .WithMany(qg => qg.QuizGameUsers)
                .HasForeignKey(qgu => qgu.QuizGameId);

            modelBuilder.Entity<QuizGameUser>()
                .HasOne(qgu => qgu.User)
                .WithMany(u => u.QuizGameUsers)
                .HasForeignKey(qgu => qgu.UserId);
        }

    }
}