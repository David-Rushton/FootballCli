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
    public class CompetitionRepository : RepositoryBase
    {
        const string CompetitionsUri = "http://api.football-data.org/v2/competitions/?plan=TIER_ONE";

        readonly ILogger<CompetitionRepository> _logger;


        public CompetitionRepository(IOptions<SourceConfig> config, ILogger<CompetitionRepository> logger)
            : base(config)
            => (_logger) = (logger)
        ;


        public async Task<FootballCompetitions> GetCompetitions()
        {
            var stream = _client.GetStreamAsync(CompetitionsUri);
            var competitions = await JsonSerializer.DeserializeAsync<FootballCompetitions>(await stream);

            // todo: better handing of failed returns - perhaps via base?
            Debug.Assert(competitions is not null, "Competitions is null");


            return competitions;
        }
    }
}
