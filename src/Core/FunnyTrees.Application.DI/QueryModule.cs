using Autofac;
using FunnyTrees.Application.Contracts.Queries;
using FunnyTrees.Application.Dispatching;
using FunnyTrees.Application.QueryHandlers.TreeNode;

namespace FunnyTrees.Application.DI;
internal class QueryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new QueryDispatcher(ctx.Resolve<IComponentContext>())).AsImplementedInterfaces();

        RegisterQueryHandlers(builder);
    }

    private void RegisterQueryHandlers(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(GetByRootNameQueryHandler).Assembly)
            .AsClosedTypesOf(typeof(IQueryHandler<,>))
            .AsImplementedInterfaces();
    }
}