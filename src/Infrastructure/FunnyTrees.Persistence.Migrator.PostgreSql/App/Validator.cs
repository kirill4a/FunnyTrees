using FunnyTrees.Persistence.Migrator.PostgreSql.Infrastructure;
using System.Text;

namespace FunnyTrees.Persistence.Migrator.PostgreSql.App;

internal static class Validator
{
    private readonly static HashSet<string> _argsAuto = new(StringComparer.OrdinalIgnoreCase)
        {
            "-a", "--auto"
        };
    private readonly static HashSet<ConsoleKey> _allowedKeys = new()
        {
            ConsoleKey.Y,
            ConsoleKey.N
        };

    internal static bool HasAutoKey(IEnumerable<string> args)
        =>
        args?.Any(a => _argsAuto.Contains(a)) ?? false;

    internal static bool UserValidate(string connectionString)
    {
        var sb = new StringBuilder($"{Environment.NewLine}")
                    .Append(GetConnectionSettings(connectionString)).AppendLine()
                    .Append($"{Environment.NewLine}Is it ok (y/n) ?");

        Console.WriteLine(sb.ToString());
        return GetUserYesNo();
    }

    private static string GetConnectionSettings(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception($"Connection '{Constants.ConnectionName}' not found. Check appsettings.json file");
        var connectionParams = connectionString
            .Split(';')
            .Where(s => s.StartsWith("Host", StringComparison.OrdinalIgnoreCase)
                   || s.StartsWith("Database", StringComparison.OrdinalIgnoreCase));

        var result = new StringBuilder($"Migrations would be applied to:{Environment.NewLine}")
                    .AppendJoin(Environment.NewLine, connectionParams)
                    .AppendLine()
                    .ToString();
        return result;
    }

    private static bool GetUserYesNo()
    {
        _ = Enum.TryParse<ConsoleKey>(Console.ReadLine(), true, out var answerKey);
        while (!_allowedKeys.Contains(answerKey))
        {
            Console.WriteLine($"{Environment.NewLine}Please, input 'y' or 'n'");
            _ = Enum.TryParse(Console.ReadLine(), true, out answerKey);
        }
        Console.WriteLine();
        return answerKey == ConsoleKey.Y;
    }
}