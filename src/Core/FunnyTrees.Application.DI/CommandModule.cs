using Autofac;
using FunnyTrees.Application.CommandHandlers.TreeNode;
using FunnyTrees.Application.Contracts.Commands;
using FunnyTrees.Application.Dispatching;

namespace FunnyTrees.Application.DI;
internal class CommandModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new CommandDispatcher(ctx.Resolve<IComponentContext>())).AsImplementedInterfaces();

        RegisterCommandHandlers(builder);
    }

    private void RegisterCommandHandlers(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(CreateTreeNodeCommandHandler).Assembly)
            .AsClosedTypesOf(typeof(ICommandHandler<,>))
            .AsImplementedInterfaces();
    }
}