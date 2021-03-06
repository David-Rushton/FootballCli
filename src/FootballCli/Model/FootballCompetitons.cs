using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballCompetitions
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("competitions")]
        public List<FootballCompetition> Competitions { get; init; } = new();
    }
}
