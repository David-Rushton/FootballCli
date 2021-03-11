using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class LeagueTable
    {
        [JsonPropertyName("season")]
        public FootballSeason Season { get; init; } = new();

        [JsonPropertyName("standings")]
        public List<LeagueStanding> Standings { get; init; } = new();
    }
}
