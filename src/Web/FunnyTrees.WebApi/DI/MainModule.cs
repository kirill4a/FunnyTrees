using Autofac;
using FunnyTrees.Application.DI;
using FunnyTrees.Persistence.DI;

namespace FunnyTrees.WebApi.DI;

internal class MainModule : Module
{
    private readonly string _connectionString;

    public MainModule(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
        _connectionString = connectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ApplicationModule>();
        builder.RegisterModule(new PersistenceModule(_connectionString));
    }
}
