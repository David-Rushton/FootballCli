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
    public class Football
    {
        const string RequestUri = "http://api.football-data.org/v2/competitions/PL/matches/?dateFrom=2021-03-04&dateTo=2021-03-04";

        readonly SourceConfig _config;

        private readonly HttpClient _client = new();


        public Football(IOptions<SourceConfig> config)
        {
            _config = config.Value;
            InitialiseClient();
        }


        public async Task<FootballMatches> GetMatches()
        {
            var stream = _client.GetStreamAsync(RequestUri);
            var matches = await JsonSerializer.DeserializeAsync<FootballMatches>(await stream);

            Debug.Assert(matches is not null, "Matches is null");


            return matches;
        }




        private void InitialiseClient()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("X-Auth-Token", _config!.ApiKey);
        }
    }
}
