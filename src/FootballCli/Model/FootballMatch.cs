using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballMatch
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("utcDate")]
        public DateTime KickOff { get; init; }

        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdate { get; init; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("matchday")]
        public int MatchDay { get; set; }

        [JsonPropertyName("homeTeam")]
        public FootballTeam HomeTeam { get; init; } = new();

        [JsonPropertyName("awayTeam")]
        public FootballTeam AwayTeam { get; init; } = new();

        [JsonPropertyName("score")]
        public FootballMatchScore Score { get; init; } = new();
    }
}
