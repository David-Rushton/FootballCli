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

        static IRenderable[] GetRow(FootballCompetitionCurrentSeason competition) =>
            new[]
            {
                new Markup(competition.Code),
                new Markup(competition.Name),
                new Markup($"{competition.CurrentSeason.StartDate:yyyy-MM-dd}"),
                new Markup($"{competition.CurrentSeason.EndDate:yyyy-MM-dd}"),
                new Markup($"{competition.CurrentSeason.CurrentMatchday ?? 0}")
            };
    }
}

