using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballMatchScore
    {
        [JsonPropertyName("winner")]
        public string Winner { get; init; } = string.Empty;

        [JsonPropertyName("duration")]
        public string Duration { get; init; } = string.Empty;

        [JsonPropertyName("fullTime")]
        public FootballMatchPeriodScore FullTime { get; init; } = new();

        [JsonPropertyName("halfTime")]
        public FootballMatchPeriodScore HalfTime { get; init; } = new();

        [JsonPropertyName("extraTime")]
        public FootballMatchPeriodScore ExtraTime { get; init; } = new();

        [JsonPropertyName("penalties")]
        public FootballMatchPeriodScore Penalties { get; init; } = new();
    }
}
