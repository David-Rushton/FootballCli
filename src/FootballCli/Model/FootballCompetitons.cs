using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Spectre.Console;


namespace FootballCli.Model
{
    public class FootballCompetitions
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("competitions")]
        public List<FootballCompetitionCurrentSeason> Items { get; init; } = new();

        public FootballCompetitionCurrentSeason this[int index] => Items[index];

        public FootballCompetitionCurrentSeason this[string code] => Items.Where(c => c.Code == code).First();


        public bool IsValidCompetitionCode(string code) => Items.Exists(c => c.Code.ToLower() == code.ToLower());
    }
}
