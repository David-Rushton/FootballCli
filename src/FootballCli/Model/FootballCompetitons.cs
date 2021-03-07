using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;


namespace FootballCli.Model
{
    public class FootballCompetitions
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("competitions")]
        public List<FootballCompetition> Competitions { get; init; } = new();


        public FootballCompetition SelectCompetition()
        {
            var sortedCompetitons = Competitions.OrderBy(c => c.Name).Select(c => c.Name);
            var competitionPrompt = new SelectionPrompt<string>()
                .PageSize(5)
                .AddChoices(sortedCompetitons)
                .Title("Select a Competition")
            ;
            var selectedCompetition = AnsiConsole.Prompt(competitionPrompt);

            return Competitions.Where(c => c.Name == selectedCompetition).First();
        }
    }
}
