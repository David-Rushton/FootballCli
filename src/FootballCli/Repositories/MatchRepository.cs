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

        readonly Dictionary<int, FootballMatch> _cachedResults = new();


        public MatchRepository(IOptions<SourceConfig> config, ILogger<MatchRepository> logger)
            : base(config)
            => (_logger) = (logger)
        ;


        public async Task<FootballMatches> GetLiveMatches(string competitionCode)
        {
            var result = await base.GetResource<FootballMatches>(GetLiveUri(competitionCode));

            // tag any match with an interesting revision (goal(s) scored, status change etc).
            // then update cache so we can spot the next time an interesting revision occurs.
            foreach(var match in result.Matches)
                if(_cachedResults.TryGetValue(match.Id, out var cachedMatch))
                {
                    if(MatchHasBeenRevised(match, cachedMatch))
                        IncrementRevisionAndUpdateCache(match, cachedMatch);
                }
                else
                    _cachedResults.Add(match.Id, match);


            return result;


            bool MatchHasBeenRevised(FootballMatch match, FootballMatch cachedMatch) =>
                match.StatusCode != cachedMatch.StatusCode || match.PrettyPrintScore() != cachedMatch.PrettyPrintScore()
            ;

            void IncrementRevisionAndUpdateCache(FootballMatch match, FootballMatch cachedMatch)
            {
                match.Revision = (cachedMatch.Revision.Number + 1, DateTime.UtcNow);
                _cachedResults[match.Id] = match;
            }
        }

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
