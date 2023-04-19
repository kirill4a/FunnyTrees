using Autofac;

namespace FunnyTrees.Application.DI;
public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ServicesModule>();
        builder.RegisterModule<CommandModule>();
        builder.RegisterModule<QueryModule>();
    }
}
