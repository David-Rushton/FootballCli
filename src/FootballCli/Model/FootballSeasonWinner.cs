namespace Dr.FootballCli.Model;

public readonly record struct FootballSeasonWinner(
    int Id,
    string Name ,
    string ShortName,
    [property: JsonPropertyName("tla")]
    string? WinnerCode,
    [property: JsonPropertyName("Crest")]
    string? CrestUrl);
