namespace Dr.FootballCli.Model;

public readonly record struct FootballCompetitionCurrentSeason(
    int Id,
    string Name,
    string Code,
    DateTime LastUpdated,
    FootballSeason CurrentSeason);
    
