namespace Dr.FootballCli.Model;

public readonly record struct FootballResultSet(
    int Count,
    [property: JsonPropertyName("First")]
    DateTime From,
    [property: JsonPropertyName("Last")]
    DateTime Until,
    int Played);
