namespace Dr.FootballCli.Model;

public readonly record struct FootballTeam(
    int Id,
    string Name,
    string ShortName,
    string Code,
    string CrestUri);
