using Microsoft.EntityFrameworkCore;

namespace TrelloClone.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<List> Lists { get; set; } = null!;
        public DbSet<TaskItem> TaskItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<List>()
                .HasOne(l => l.Board)
                .WithMany(b => b.Lists)
                .HasForeignKey(l => l.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.List)
                .WithMany(l => l.Tasks)
                .HasForeignKey(t => t.ListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
