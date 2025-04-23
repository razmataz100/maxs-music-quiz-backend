using MaxsMusicQuiz.Backend.Data.Entities;
using MaxsMusicQuiz.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaxsMusicQuiz.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Pass the options to the base constructor
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<QuizGame> QuizGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuizGame>()
                .HasMany(g => g.TeamA)
                .WithMany(u => u.Games)
                .UsingEntity(j => j.ToTable("TeamAGameUsers"));

            modelBuilder.Entity<QuizGame>()
                .HasMany(g => g.TeamB)
                .WithMany(u => u.Games)
                .UsingEntity(j => j.ToTable("TeamBGameUsers"));
        }
    }
}