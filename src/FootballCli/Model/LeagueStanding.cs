using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class LeagueStanding
    {
        [JsonPropertyName("stage")]
        public string Stage { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;

        [JsonPropertyName("group")]
        public string? Group { get; init; } = string.Empty;

        [JsonPropertyName("table")]
        public List<LeaguePosition> Positions { get; init; } = new();
    }
}
