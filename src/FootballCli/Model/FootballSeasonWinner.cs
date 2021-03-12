using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballSeasonWinner
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("shortName")]
        public string ShortName { get; init; } = string.Empty;

        [JsonPropertyName("tla")]
        public string ThreeLetterAbbreviation { get; init; } = string.Empty;

        [JsonPropertyName("crestUrl")]
        public string CrestUrl { get; init; } = string.Empty;
    }
}
