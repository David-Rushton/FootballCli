using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dr.FootballCli.Commands;
using Dr.FootballCli.Config;
using Dr.FootballCli.Repositories;
using Dr.FootballCli.Views;

namespace Dr.FootballCli;

public static class Startup
{
    public static IServiceCollection AddFootballCli(this IServiceCollection serviceCollection, IConfigurationRoot config)
    {
        serviceCollection
            .Configure<SourceConfig>(config.GetSection("source"))
            .Configure<FavouriteTeamConfig>(config.GetSection("favouriteTeam"))
            .AddTransient<TableViewFactory>()
            .AddTransient<CompetitionRepository>()
            .AddTransient<LeagueRepository>()
            .AddTransient<MatchRepository>()
        ;


        return serviceCollection;
    }

    public static IConfigurator UseEnvironmentSpecificConfig(
        this IConfigurator config,
        string hostEnvironmentName
    )
    {
        if(hostEnvironmentName.ToLower() == "development")
            config
                // smoke testing relies on ValidateExamples to ensure commands and examples are aligned
                .ValidateExamples()
                .PropagateExceptions()
        ;


        return config;
    }

    public static IConfigurator AddCommands(this IConfigurator config)
    {
        config.AddCommand<CompetitionCommand>("competition")
            .WithDescription("View available competitions")
            .WithExample(new[] { "competition" })
        ;

        config.AddCommand<LiveScoresCommand>("live")
            .WithDescription("View live scores")
            .WithExample(new[] { "live", "-f", "--follow" })
        ;

        config.AddCommand<TableCommand>("table")
            .WithDescription("View league standings")
            .WithExample(new[] { "table", "-f", "--full-table" })
        ;


        return config;
    }
}
