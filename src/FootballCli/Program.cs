using Dr.FootballCli.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


await Bootstrap().RunAsync(args);


CommandApp Bootstrap()
{
    var configuration = new ConfigurationBuilder()
        // TOOD: This isn't always the execution folder.
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"AppSettings.json", optional: true)
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
                .AddCommands();

            // Used during PRs to ensure help documentation is up to date.
            if (Environment.GetEnvironmentVariable("FOOTBALL_CLI_VALIDATE_EXAMPLES") is not null)
                config.ValidateExamples();
        }
    );

    return app;
}
