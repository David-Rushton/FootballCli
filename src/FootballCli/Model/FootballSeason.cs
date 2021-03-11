using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballSeason
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; init; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; init; }

        [JsonPropertyName("currentMatchday")]
        public int CurrentMatchday { get; init; }

        [JsonPropertyName("winner")]
        public string? Winner { get; init; }
    }
}
