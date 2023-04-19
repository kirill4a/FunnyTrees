using Autofac;
using FunnyTrees.Domain.Repositories;
using FunnyTrees.Persistence.Entities.Models;
using FunnyTrees.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FunnyTrees.Persistence.DI;
public class PersistenceModule : Module
{
    private readonly string _connectionString;

    public PersistenceModule(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
        _connectionString = connectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterDbContext(builder);
        RegisterRepositories(builder);
    }

    private void RegisterDbContext(ContainerBuilder builder)
    {
        builder.Register(_ => new DbContextOptionsBuilder<FunnyTreesDbContext>()
                                   .UseNpgsql(_connectionString)
                                   .Options)
               .SingleInstance();

        builder.Register(ctx => new FunnyTreesDbContext(ctx.Resolve<DbContextOptions<FunnyTreesDbContext>>()))
               .AsSelf()
               .As<IUnitOfWork>();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var dbContext = ctx.Resolve<FunnyTreesDbContext>();
            return new TreeNodeRepository(dbContext.Set<TreeNode>(), dbContext);
        })
        .AsImplementedInterfaces();

        builder.Register(ctx =>
        {
            var dbContext = ctx.Resolve<FunnyTreesDbContext>();
            return new LogEntryRepository(dbContext.Set<LogEntry>(), dbContext);
        })
        .AsImplementedInterfaces();
    }
}