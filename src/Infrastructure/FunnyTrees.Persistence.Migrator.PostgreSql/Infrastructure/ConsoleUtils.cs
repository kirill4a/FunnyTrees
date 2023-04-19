namespace FunnyTrees.Persistence.Migrator.PostgreSql.Infrastructure;

internal static class ConsoleUtils
{
    internal static void WriteLineRed(string text) => WriteLineWithColor(text, ConsoleColor.Red);
    internal static void WriteLineYellow(string text) => WriteLineWithColor(text, ConsoleColor.Yellow);
    internal static void WriteLineGreen(string text) => WriteLineWithColor(text, ConsoleColor.Green);

    private static void WriteLineWithColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
