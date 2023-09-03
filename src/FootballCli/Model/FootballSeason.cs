namespace Dr.FootballCli.Model;

public readonly record struct FootballSeason(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    int? CurrentMatchday,
    FootballSeasonWinner? Winner);
