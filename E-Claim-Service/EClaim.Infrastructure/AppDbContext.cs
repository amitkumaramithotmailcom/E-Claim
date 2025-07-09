using Microsoft.EntityFrameworkCore;
using EClaim.Domain.Entities;

namespace EClaim.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ClaimDocument> ClaimDocuments { get; set; }
    public DbSet<ClaimRequest> Claims { get; set; }
    public DbSet<ClaimWorkflowStep> ClaimWorkflowSteps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Fluent API configs
    }
}