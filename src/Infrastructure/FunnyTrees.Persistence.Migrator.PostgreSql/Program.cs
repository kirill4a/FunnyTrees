using FunnyTrees.Persistence.Migrator.PostgreSql.App;
using FunnyTrees.Persistence.Migrator.PostgreSql.Infrastructure;

Console.WriteLine(typeof(Program).Assembly.FullName);

var isAuto = Validator.HasAutoKey(args);
var connectionString = new Configurator().GetConnectionString(Constants.ConnectionName);
try
{
    if (isAuto)
    {
        var resultCode = Runner.RunMigrations(connectionString);
        Environment.Exit(resultCode);
        return;
    }

    if (Validator.UserValidate(connectionString))
        Runner.RunMigrations(connectionString);
    else
        ConsoleUtils.WriteLineYellow(
            $"Migrations has been canceled by {Environment.UserDomainName}\\{Environment.UserName}");
}
catch (Exception ex)
{
    ConsoleUtils.WriteLineRed($"Error occurred:{Environment.NewLine}{ex}");
    if (isAuto)
    {
        Environment.Exit(1);
        return;
    }
}
Console.WriteLine("Press any key to exit ....");
Console.ReadKey(true);