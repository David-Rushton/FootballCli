using FootballCli.Config;
using FootballCli.Commands;
using FootballCli.Model;
using FootballCli.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.IO;
using System.Threading.Tasks;


namespace FootballCli
{
    class Program
    {
        static async Task<int> Main(string[] args) =>
            await Bootstrap().RunAsync(args)
        ;


        static CommandApp Bootstrap()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build()
            ;

            var serviceCollection = new ServiceCollection()
                .AddLogging
                (
                    config =>
                    {
                        config.AddSimpleConsole();
                    }
                )
                .Configure<SourceConfig>(configuration.GetSection("source"))
                .AddTransient<CompetitionRepository>()
                .AddTransient<LeagueRepository>()
                .AddTransient<MatchRepository>()
            ;


            using var registrar = new DependencyInjectionRegistrar(serviceCollection);
            var app = new CommandApp(registrar);


            app.Configure
            (
                config =>
                {
                    config.Settings.ApplicationName = "Football cli";
                    config.ValidateExamples();

                    // todo: _maybe_ enable in dev mode.
                    config.Settings.PropagateExceptions = true;


                    config.AddCommand<CompetitionCommand>("competition")
                        .WithDescription("View available competitions")
                        .WithExample(new[] { "competition" })
                    ;

                    config.AddCommand<LiveScoresCommand>("live")
                        .WithDescription("View live scores")
                        .WithExample(new[] { "live", "-f", "--follow-live" })
                    ;

                    config.AddCommand<TableCommand>("table")
                        .WithDescription("View league standings")
                        .WithExample(new[] { "table" })
                    ;
                }
            );

            return app;
        }
    }
}
