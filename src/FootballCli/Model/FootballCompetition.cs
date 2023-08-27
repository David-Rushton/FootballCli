namespace Dr.FootballCli.Model;

public readonly record struct FootballCompetition(
    int Id,
    string Name,
    string Code,
    DateTime LastUpdated);
