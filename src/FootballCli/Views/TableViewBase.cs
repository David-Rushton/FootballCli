using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using FootballCli.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Spectre.Console.Rendering;


namespace FootballCli.Views
{
    public abstract class TableViewBase
    {
        readonly FavouriteTeamConfig _favouriteTeamConfig;


        public TableViewBase(IOptions<FavouriteTeamConfig> favouriteTeamConfig) =>
            _favouriteTeamConfig = favouriteTeamConfig.Value
        ;


        public abstract void RenderTable(LeagueTable leagueTable, bool showFullTable);

        protected void RenderTable(LeagueTable leagueTable, int[]? seperatorRows, bool showFullTable)
        {
            var winner = leagueTable.Season.Winner?.Name ?? string.Empty;
            var competitionName = leagueTable.Competition.Name;
            var lastUpdated = leagueTable.Competition.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");

            foreach(var standing in leagueTable.Standings)
            {
                var tableTitle = $"[bold yellow]{competitionName} {standing.PrettyPrintGroup} {lastUpdated}[/]";
                var positions = standing.Positions.OrderBy(p => p.Position);

                var table = new Table()
                    .Caption(tableTitle)
                    .Border(TableBorder.Rounded)
                    .AddWinner(winner)
                    .AddFavouriteTeam(_favouriteTeamConfig.Name, _favouriteTeamConfig.Colour)
                    .ShouldShowFullTable(showFullTable)
                    .AddHeaderRow()
                    .AddDetailRows(standing, seperatorRows)
                ;

                AnsiConsole.Render(table);
            }
        }
    }
}
