using System.Globalization;

namespace Dr.FootballCli.Model;

public readonly record struct LeagueStanding(
    string Stage,
    string Type,
    string? Group,
    List<LeaguePosition> Positions);
