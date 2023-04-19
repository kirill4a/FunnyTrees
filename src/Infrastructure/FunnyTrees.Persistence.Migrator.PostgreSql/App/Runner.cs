using DbUp;
using DbUp.Engine;
using FunnyTrees.Persistence.Migrator.PostgreSql.Infrastructure;
using System.Reflection;

namespace FunnyTrees.Persistence.Migrator.PostgreSql.App;

internal static class Runner
{
    internal static int RunMigrations(string connectionString)
    {
        Console.WriteLine("Starting migration....");

        var upgrader = GetUpgradeEngine(connectionString);
        return UpdateDatabase(upgrader);
    }

    private static UpgradeEngine GetUpgradeEngine(string connectionString)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName().Name;
        var schemaScriptPrefix = $"{assemblyName}.{Constants.PrefixSchemaScript}";

        var upgrader =
           DeployChanges.To
               .PostgresqlDatabase(connectionString)
               .WithScriptsEmbeddedInAssembly(assembly,
                                              script => script.StartsWith(schemaScriptPrefix),
                                              new SqlScriptOptions()
                                              {
                                                  ScriptType = DbUp.Support.ScriptType.RunOnce,
                                                  RunGroupOrder = 0
                                              })
               .LogToConsole()
               .Build();

        return upgrader;
    }

    private static int UpdateDatabase(UpgradeEngine engine)
    {
        var result = engine.PerformUpgrade();

        if (!result.Successful)
        {
            ConsoleUtils.WriteLineRed(result.Error.ToString());
            return 1;
        }
        ConsoleUtils.WriteLineGreen("Database is of the latest version now");
        return 0;
    }
}
