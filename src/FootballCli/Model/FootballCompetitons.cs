namespace Dr.FootballCli.Model;

public readonly record struct FootballCompetitions(
    int Count,
    [property: JsonPropertyName("competitions")]
    List<FootballCompetitionCurrentSeason> Items)
{
    public FootballCompetitionCurrentSeason this[int index] =>
        Items[index];

     public FootballCompetitionCurrentSeason this[string code] =>
         Items.First(c => c.Code == code);

     public bool IsValidCompetitionCode(string code) =>
         Items.Exists(c => string.Equals(c.Code, code, StringComparison.CurrentCultureIgnoreCase));
}
