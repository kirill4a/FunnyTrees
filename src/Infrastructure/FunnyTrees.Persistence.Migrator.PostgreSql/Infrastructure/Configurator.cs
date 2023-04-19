using Microsoft.Extensions.Configuration;

namespace FunnyTrees.Persistence.Migrator.PostgreSql.Infrastructure;

internal class Configurator
{
    private readonly IConfiguration _configuration;

    public Configurator()
    {
        _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }

    internal string GetConnectionString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (_configuration == null)
            throw new NullReferenceException("Configuration shouldn\'t be null");

        var result = _configuration.GetConnectionString(key);
        return !string.IsNullOrWhiteSpace(result)
            ? result
            : throw new Exception($"Connection string with key '{key}' was not found or empty");
    }
}