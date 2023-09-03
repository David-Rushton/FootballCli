namespace Dr.FootballCli.Model;

public readonly record struct FootballCompetitionCurrentSeason(
    int Id,
    string Name,
    string? Code,
    string Type,
    [property: JsonPropertyName("emblem")]
    string LogoUri,
    DateTime LastUpdated,
    FootballSeason? CurrentSeason);

