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
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace FootballCli
{
    class Program
    {
        static string HostEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";


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
                    config.UseStrictParsing();

                    if(HostEnvironment == "Development")
                    {
                        // smoke testing required to ensure examples are always valid
                        config.ValidateExamples();
                        config.Settings.PropagateExceptions = true;
                    }


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
                }
            );

            return app;
        }
    }
}
