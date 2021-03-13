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


namespace FootballCli.Views
{
    public abstract class TableViewBase
    {
        public virtual void RenderTable(LeagueTable leagueTable) =>
            RenderTable(leagueTable, seperatorRows: null)
        ;


        protected void RenderTable(LeagueTable leagueTable, int[]? seperatorRows)
        {
            var winner = leagueTable.Season.Winner?.Name ?? string.Empty;
            var competitionName = leagueTable.Competition.Name;
            var lastUpdated = leagueTable.Competition.LastUpdated.ToString("yyyy-MM-dd hh:mm:ss");
            var standings = leagueTable.Standings.Where(s => s.Type == "TOTAL").First();
            var positions = standings.Positions.OrderBy(p => p.Position);
            var table = new Table()
                .Caption($"[bold yellow]{competitionName}[/] - [yellow]{lastUpdated}[/]")
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


            foreach(var position in positions)
            {
                table.AddRow
                (
                    position.Position.ToString(),
                    FormatTeamName(position.Team.Name, winner),
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

                if(seperatorRows?.Contains(position.Position) ?? false)
                    table.AddEmptyRow()
                ;
            }

            AnsiConsole.Render(table);
        }


        private string FormatTeamName(string teamName, string winner) =>
            teamName == winner ? $"[bold yellow]{teamName}[/]" : teamName
        ;

        private string FormatGoalDifference(int goalDifference) =>
            goalDifference switch
            {
                int difference when difference > 0 => $"[green]{difference}[/]",
                int difference when difference < 0 => $"[red]{difference}[/]",
                _                                  =>  "[lightslategrey]0[/]"
            }
        ;

        private string FormatForm(string form) =>
            form
                .Replace(",", string.Empty)
                .Replace("W", "[green]W[/]")
                .Replace("D", "[lightslategrey]D[/]")
                .Replace("L", "[red]L[/]")
        ;
    }
}
