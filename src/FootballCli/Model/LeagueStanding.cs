using System.Globalization;

namespace Dr.FootballCli.Model;

public readonly record struct LeagueStanding(
    string Stage,
    string Type,
    string? Group,
    List<LeaguePosition> Positions)
{
    public string PrettyPrintGroup =>
        new CultureInfo("en-GB")
            .TextInfo
            .ToTitleCase(Group?.ToLower() ?? string.Empty)
            .Replace('_', ' ');
}
