using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
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
        const string MatchdayUriTemplate = "v2/competitions/<COMPETITION_CODE>/matches/?matchday=<MATCHDAY>";

        const string LiveUriTemplate = "v2/competitions/<COMPETITION_CODE>/matches/?dateFrom=<TODAY>&dateTo=<TODAY>";

        readonly ILogger<MatchRepository> _logger;


        public MatchRepository(IOptions<SourceConfig> config, ILogger<MatchRepository> logger)
            : base(config)
            => (_logger) = (logger)
        ;


        public async Task<FootballMatches> GetLiveMatches(string competitionCode) =>
            await base.GetResource<FootballMatches>(GetLiveUri(competitionCode))
        ;

        public async Task<FootballMatches> GetMatchesByMatchday(string competitionCode, int matchday) =>
            await base.GetResource<FootballMatches>(GetMatchdayUri(competitionCode, matchday))
        ;


        private string GetLiveUri(string competitionCode) =>
            LiveUriTemplate
                .Replace("<COMPETITION_CODE>", competitionCode.ToUpper())
                .Replace("<TODAY>", DateTime.Now.ToString("yyyy-MM-dd"))
        ;

        private string GetMatchdayUri(string competitionCode, int matchday) =>
            LiveUriTemplate
                .Replace("<COMPETITION_CODE>", competitionCode.ToUpper())
                .Replace("<MATCHDAY>", matchday.ToString())
        ;
    }
}
