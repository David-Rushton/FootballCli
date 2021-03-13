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
        static string HostEnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";


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

            using var registrar = new DependencyInjectionRegistrar(serviceCollection);
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
}
