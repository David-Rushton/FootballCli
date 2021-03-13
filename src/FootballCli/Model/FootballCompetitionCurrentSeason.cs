using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballCompetitionCurrentSeason : FootballCompetition
    {
        [JsonPropertyName("currentSeason")]
        public FootballSeason CurrentSeason { get; set; } = new();
    }
}
