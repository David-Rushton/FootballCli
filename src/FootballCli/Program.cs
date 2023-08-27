using Dr.FootballCli.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Dr.FootballCli;

internal static class Program
{
    private static readonly string HostEnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

    static async Task<int> Main(string[] args) =>
        await Bootstrap().RunAsync(args)
    ;


    static CommandApp Bootstrap()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables("fbcli")
            .Build()
        ;

        var serviceCollection = new ServiceCollection()
            .AddLogging(config => config.AddSimpleConsole())
            .AddFootballCli(configuration)
        ;

        var registrar = new TypeRegistrar(serviceCollection);
        var app = new CommandApp(registrar);

        app.Configure
        (
            config =>
            {
                config
                    .SetApplicationName("Football Cli")
                    .UseStrictParsing()
                    .UseEnvironmentSpecificConfig(HostEnvironmentName)
                    .AddCommands()
                ;
            }
        );


        return app;
    }
}
