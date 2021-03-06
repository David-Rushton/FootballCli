using FootballCli.Config;
using FootballCli.Commands.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace FootballCli.Model
{
    // todo: caching
    public class Football
    {
        const string CompetitionsUri = "http://api.football-data.org/v2/competitions/?plan=TIER_ONE";

        const string MatchesUri = "http://api.football-data.org/v2/competitions/<COMPETITION_CODE>/matches/?matchday=<MATCHDAY>";


        readonly SourceConfig _config;

        private readonly HttpClient _client = new();


        public Football(IOptions<SourceConfig> config)
        {
            _config = config.Value;
            InitialiseClient();
        }


        public async Task<FootballMatches> GetMatches(string competitionCode, int matchday)
        {
            var stream = _client.GetStreamAsync(MatchesUri.Replace("<COMPETITION_CODE>", competitionCode.ToUpper()).Replace("<MATCHDAY>", matchday.ToString()));
            var matches = await JsonSerializer.DeserializeAsync<FootballMatches>(await stream);

            Debug.Assert(matches is not null, "Matches is null");


            return matches;
        }

        public async Task<FootballCompetitions> GetCompetitions()
        {
            var stream = _client.GetStreamAsync(CompetitionsUri);
            var competitions = await JsonSerializer.DeserializeAsync<FootballCompetitions>(await stream);

            Debug.Assert(competitions is not null, "Competitions is null");


            return competitions;
        }




        private void InitialiseClient()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("X-Auth-Token", _config!.ApiKey);
        }
    }
}
