using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballMatches
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("matches")]
        public List<FootballMatch> Matches { get; set; } = new();
    }
}
