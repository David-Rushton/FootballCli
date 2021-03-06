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
    public class LeagueRepository : RepositoryBase
    {
        const string MatchesUri = "http://api.football-data.org/v2/competitions/<COMPETITION_CODE>/standings/?standingType=TOTAL";

        readonly ILogger<LeagueRepository> _logger;


        public LeagueRepository(IOptions<SourceConfig> config, ILogger<LeagueRepository> logger)
            : base(config)
            => (_logger) = (logger)
        ;


        public async Task<LeagueTable> GetLeagueTable(string competitionCode)
        {
            var uri = MatchesUri.Replace("<COMPETITION_CODE>", competitionCode.ToUpper());
            return await base.GetResource<LeagueTable>(uri);
        }
    }
}
