using Autofac;
using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;
using FunnyTrees.Application.Services.ExceptionHandling;

namespace FunnyTrees.Application.DI;

internal class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterExceptionHandlers(builder);

        builder.Register(ctx => new ExceptionHandlerStrategy(
            ctx.ResolveKeyed<IExceptionHandler>(nameof(SecureExceptionHandler)),
            ctx.ResolveKeyed<IExceptionHandler>(nameof(RegularExceptionHandler))
            ))
            .AsImplementedInterfaces();
    }

    private void RegisterExceptionHandlers(ContainerBuilder builder)
    {
        builder.Register(ctx => new RegularExceptionHandler(ctx.Resolve<ICommandDispatcher>()))
               .Keyed<IExceptionHandler>(nameof(RegularExceptionHandler));

        builder.Register(ctx => new SecureExceptionHandler(ctx.Resolve<ICommandDispatcher>()))
               .Keyed<IExceptionHandler>(nameof(SecureExceptionHandler));
    }
}