using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballMatchPeriodScore
    {
        [JsonPropertyName("homeTeam")]
        public int? HomeTeam { get; init; }

        [JsonPropertyName("awayTeam")]
        public int? AwayTeam { get; init; }
    }
}
