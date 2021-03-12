using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
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


namespace FootballCli.Repositories
{
    public class MatchRepository : RepositoryBase
    {
        const string MatchesUri = "http://api.football-data.org/v2/competitions/<COMPETITION_CODE>/matches/?matchday=<MATCHDAY>";

        readonly ILogger<MatchRepository> _logger;


        public MatchRepository(IOptions<SourceConfig> config, ILogger<MatchRepository> logger)
            : base(config)
            => (_logger) = (logger)
        ;


        public async Task<FootballMatches> GetMatches(string competitionCode, int matchday)
        {
            var uri = MatchesUri.Replace("<COMPETITION_CODE>", competitionCode.ToUpper()).Replace("<MATCHDAY>", matchday.ToString());
            var stream = _client.GetStreamAsync(uri);
            var matches = await JsonSerializer.DeserializeAsync<FootballMatches>(await stream);

            // todo: non-200s
            Debug.Assert(matches is not null, "Matches is null");


            return matches;
        }
    }
}
