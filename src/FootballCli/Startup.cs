using FootballCli.Config;
using FootballCli.Commands;
using FootballCli.Model;
using FootballCli.Repositories;
using FootballCli.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;


namespace FootballCli
{
    public static class Startup
    {
        public static IServiceCollection AddFootballCli(this IServiceCollection serviceCollection, IConfigurationRoot config)
        {
            serviceCollection
                .Configure<SourceConfig>(config.GetSection("source"))
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
                .WithExample(new[] { "table" })
            ;


            return config;
        }
    }
}
