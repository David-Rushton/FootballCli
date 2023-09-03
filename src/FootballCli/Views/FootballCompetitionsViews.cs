using Spectre.Console.Rendering;

namespace Dr.FootballCli.Views;

public static class FootballCompetitionsViews
{
    public static Table ToTable(this FootballCompetitions competitions)
    {
        var table = new Table()
            .MarkdownBorder()
            .AddColumn("Code")
            .AddColumn("Name")
            .AddColumn("Starts")
            .AddColumn("Ends")
            .AddColumn("Match Day");


        foreach (var competition in competitions.Items)
            table.AddRow(GetRow(competition));

        return table;

        static IRenderable[] GetRow(FootballCompetitionCurrentSeason competition)
        {
            var seasonDetails = competition.CurrentSeason is FootballSeason season
                ? new
                {
                    StartDate = season.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = season.EndDate.ToString("yyyy-MM-dd"),
                    MatchDay = season.CurrentMatchday.ToString() ?? string.Empty
                }
                : new
                {
                    StartDate = string.Empty,
                    EndDate = string.Empty,
                    MatchDay = string.Empty
                };

            return new[]
            {
                new Markup(competition.Code ?? string.Empty),
                new Markup(competition.Name),
                new Markup($"{seasonDetails.StartDate:yyyy-MM-dd}"),
                new Markup($"{seasonDetails.EndDate:yyyy-MM-dd}"),
                new Markup($"{seasonDetails.MatchDay}")
            };
        }
    }
}

