using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using FootballCli.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Linq;
using System.Threading.Tasks;


namespace FootballCli.Commands
{
    public class TableCommand : AsyncCommand<TableSettings>
    {
        readonly ILogger<TableCommand> _logger;

        readonly LeagueRepository _leagueRepository;


        public TableCommand(ILogger<TableCommand> logger, LeagueRepository leagueRepository) =>
            (_logger, _leagueRepository) = (logger, leagueRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context, TableSettings settings)
        {
            var leagueTable = await _leagueRepository.GetLeagueTable(settings.CompetitionCode);
            var standing = leagueTable.Standings.Where(s => s.Type == "TOTAL").First();

            var table = new Table()
                // todo: league competition name + better colour and position.
                .Caption("As it stands")
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("Position").RightAligned())
                .AddColumn(new TableColumn("Team").LeftAligned())
                .AddColumn(new TableColumn("Games Played").RightAligned())
                .AddColumn(new TableColumn("Won").RightAligned())
                .AddColumn(new TableColumn("Drawn").RightAligned())
                .AddColumn(new TableColumn("Lost").RightAligned())
                .AddColumn(new TableColumn("Goals For").RightAligned())
                .AddColumn(new TableColumn("Goals Against").RightAligned())
                .AddColumn(new TableColumn("Goal Difference").RightAligned())
                .AddColumn(new TableColumn("Form").Centered())
                .AddColumn(new TableColumn("Points").RightAligned())
            ;

            foreach(var position in standing.Positions.OrderBy(p => p.Position))
            {
                table.AddRow
                (
                    // todo: gold for the winners, red for relegated.  CL places in ?blue?.
                    position.Position.ToString(),
                    position.Team.Name,
                    position.PlayedGames.ToString(),
                    position.Won.ToString(),
                    position.Drawn.ToString(),
                    position.Lost.ToString(),
                    position.GoalsFor.ToString(),
                    position.GoalsAgainst.ToString(),
                    FormatGoalDifference(position.GoalDifference),
                    FormatForm(position.Form),
                    position.Points.ToString()
                );

                // todo: this only works for the Prem.
                // todo: replace with TableFormatters, for selected competitions (CL, ECL and PL + generic)
                if(position.Position is 4 or 17)
                    table.AddEmptyRow();
            }

            AnsiConsole.Render(table);

            return 0;


            string FormatGoalDifference(int goalDifference) =>
                goalDifference switch
                {
                    int difference when difference > 0 => $"[green]{difference}[/]",
                    int difference when difference < 0 => $"[red]{difference}[/]",
                    _                                  =>  "[lightslategrey]0[/]"
                }
            ;

            string FormatForm(string form) =>
                form
                    .Replace(",", string.Empty)
                    .Replace("W", "[green]W[/]")
                    .Replace("D", "[lightslategrey]D[/]")
                    .Replace("L", "[red]L[/]")
            ;
        }
    }
}
