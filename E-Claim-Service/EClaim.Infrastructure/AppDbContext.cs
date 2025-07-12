using Microsoft.EntityFrameworkCore;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;

namespace EClaim.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ClaimDocument> ClaimDocuments { get; set; }
    public DbSet<ClaimRequest> Claims { get; set; }
    public DbSet<ClaimWorkflowStep> ClaimWorkflowSteps { get; set; }
    public DbSet<AppLog> Logs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClaimWorkflowStep>()
                .HasOne(ws => ws.User)
                .WithMany()
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);
        // Fluent API configs
    }
}