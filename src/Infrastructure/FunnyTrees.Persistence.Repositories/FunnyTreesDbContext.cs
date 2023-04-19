using FunnyTrees.Domain.Repositories;
using FunnyTrees.Persistence.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace FunnyTrees.Persistence;

internal class FunnyTreesDbContext : DbContext, IUnitOfWork
{
    public FunnyTreesDbContext(DbContextOptions<FunnyTreesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreeNode).Assembly);
    }

    public DbSet<TreeNode> TreeNodes => Set<TreeNode>();
    public DbSet<LogEntry> LogEntries => Set<LogEntry>();

    public async Task CommitAsync(CancellationToken cancellation)
        =>
        _ = await SaveChangesAsync(cancellation).ConfigureAwait(false);
}
