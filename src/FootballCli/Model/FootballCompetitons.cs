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
        const string CompetitionsUri = "http://api.football-data.org/v2/competitions/?plan=TIER_ONE";

        static FootballCompetitions? _footballCompetitions;


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


        static public async Task<FootballCompetitions> Get()
        {
            if(_footballCompetitions is null)
            {
                var client = new HttpClient();
                var stream = client.GetStreamAsync(CompetitionsUri);
                _footballCompetitions = await JsonSerializer.DeserializeAsync<FootballCompetitions>(await stream);

                if(_footballCompetitions is null)
                    throw new Exception("Cannot fetch competitions");
            }

            return _footballCompetitions;
        }
    }
}
