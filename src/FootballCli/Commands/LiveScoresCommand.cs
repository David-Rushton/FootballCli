using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading.Tasks;


namespace FootballCli.Commands
{
    public class LiveScoresCommand : AsyncCommand<LiveScoresSettings>
    {
        readonly ILogger<LiveScoresCommand> _logger;

        readonly Football _football;


        public LiveScoresCommand(ILogger<LiveScoresCommand> logger, Football football) =>
            (_logger, _football) = (logger, football)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context, LiveScoresSettings settings)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("Home").RightAligned());
            table.AddColumn(new TableColumn("Score").Centered());
            table.AddColumn("Away");

            var matches = await _football.GetMatches();
            foreach(var match in matches.Matches)
            {
                var colour = PrettyPrintColour(match.Status);
                table.AddRow
                (
                    $"[{colour}]{match.HomeTeam.Name}[/]",
                    $"[{colour}]{match.Score.FullTime.HomeTeam} - {match.Score.FullTime.AwayTeam}[/]",
                    $"[{colour}]{match.AwayTeam.Name}[/]"
                );
            }


            AnsiConsole.Render(table);
            return 0;


            string PrettyPrintColour(string status) => status == "FINISHED" ? "yellow" : "white";
        }
    }
}
