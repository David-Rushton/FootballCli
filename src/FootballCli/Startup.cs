using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dr.FootballCli;

public static class Startup
{
    public static IServiceCollection AddFootballCli(this IServiceCollection serviceCollection, IConfigurationRoot config)
    {
        serviceCollection
            .Configure<ApiOptions>(config.GetSection("source"))
            .AddSingleton<Client>();

        return serviceCollection;
    }

    public static IConfigurator AddCommands(this IConfigurator config)
    {
        config
            .AddCommand<CompetitionsCommand>("competitions")
            .WithDescription("Lists supported competitions.")
            .WithAlias("competition")
            .WithAlias("comps")
            .WithAlias("comp")
            .WithAlias("leagues")
            .WithAlias("league")
            .WithExample(new[] { "competitions" });

        config
            .AddCommand<MatchesCommand>("matches")
            .WithDescription("Lists supported competitions.")
            .WithAlias("fixtures")
            .WithAlias("fixture")
            .WithAlias("results")
            .WithAlias("result")
            .WithAlias("match")
            .WithExample(new[] { "matches", "pl", "--during", "today" });

        return config;
    }
}
