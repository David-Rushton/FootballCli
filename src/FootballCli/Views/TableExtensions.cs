using Dr.FootballCli.Config;
using Dr.FootballCli.Commands.Settings;
using Dr.FootballCli.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Dr.FootballCli.Model;


namespace Dr.FootballCli.Views
{
    public static class TableExtensions
    {
        private static string _winner = string.Empty;

        private static (string Name, string Colour) _favouriteTeam = (string.Empty, string.Empty);

        public static bool _showFullTable;


        public static Table AddWinner(this Table table, string winner)
        {
            _winner = winner;
            return table;
        }

        public static Table AddFavouriteTeam(this Table table, string name, string colour)
        {
            _favouriteTeam = (name, colour);
            return table;
        }

        public static Table ShouldShowFullTable(this Table table, bool showFullTable)
        {
            _showFullTable = showFullTable;
            return table;
        }

        public static Table AddHeaderRow(this Table table)
        {
            foreach(var item in GetColumns())
                table.AddColumn(item.Column);

            return table;
        }

        public static Table AddDetailRows(this Table table, LeagueStanding standing, int[]? seperatorRows)
        {
            foreach(var position in standing.Positions)
            {
                table.AddRow(GetContents(position));

                if(seperatorRows?.Contains(position.Position) ?? false)
                    table.AddEmptyRow();
            }


            return table;

            IEnumerable<IRenderable> GetContents(LeaguePosition position)
            {
                foreach (var item in GetColumns())
                    yield return item.Content(position);
            }
        }


        private static IEnumerable<(TableColumn Column, Func<LeaguePosition, Markup> Content)> GetColumns()
        {
            yield return ( new TableColumn("Position"),     (LeaguePosition lp) => new Markup(lp.Position.ToString())       );
            yield return ( new TableColumn("Team"),         (LeaguePosition lp) => new Markup(FormatTeamName(lp.Team.Name)) );
            yield return ( new TableColumn("Games Played"), (LeaguePosition lp) => new Markup(lp.PlayedGames.ToString())    );
            yield return ( new TableColumn("Won"),          (LeaguePosition lp) => new Markup(lp.Won.ToString())            );

            if(_showFullTable)
            {
                yield return ( new TableColumn("Drawn"),           (LeaguePosition lp) => new Markup(lp.Drawn.ToString())                     );
                yield return ( new TableColumn("Lost"),            (LeaguePosition lp) => new Markup(lp.Lost.ToString())                      );
                yield return ( new TableColumn("Goals For"),       (LeaguePosition lp) => new Markup(lp.GoalsFor.ToString())                  );
                yield return ( new TableColumn("Goals Against"),   (LeaguePosition lp) => new Markup(lp.GoalsAgainst.ToString())              );
                yield return ( new TableColumn("Goal Difference"), (LeaguePosition lp) => new Markup(FormatGoalDifference(lp.GoalDifference)) );
            }

            yield return ( new TableColumn("Form"),   (LeaguePosition lp) => new Markup(FormatForm(lp.Form))  );
            yield return ( new TableColumn("Points"), (LeaguePosition lp) => new Markup(lp.Points.ToString()) );
        }

        private static string FormatTeamName(string teamName)
        {
            if(teamName == _winner)
                return $"[yellow bold]{teamName}[/]";

            if(teamName == _favouriteTeam.Name)
                return $"[{_favouriteTeam.Colour} bold]{teamName}[/]";


            return teamName;
        }

        private static string FormatGoalDifference(int goalDifference) =>
            goalDifference switch
            {
                int difference when difference > 0 => $"[green]{difference}[/]",
                int difference when difference < 0 => $"[red]{difference}[/]",
                _                                  =>  "[lightslategrey]0[/]"
            }
        ;

        private static string FormatForm(string? form) =>
            (form ?? string.Empty)
                .Replace(",", string.Empty)
                .Replace("W", "[green]W[/]")
                .Replace("D", "[lightslategrey]D[/]")
                .Replace("L", "[red]L[/]")
        ;
    }
}
