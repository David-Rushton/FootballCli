using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dr.FootballCli.Options;
using Dr.FootballCli.Model;

namespace Dr.FootballCli.Repositories;

public class LeagueRepository : RepositoryBase
{
    const string MatchesUri = "http://api.football-data.org/v2/competitions/<COMPETITION_CODE>/standings/?standingType=TOTAL";

    readonly ILogger<LeagueRepository> _logger;

    public LeagueRepository(IOptions<ApiOptions> config, ILogger<LeagueRepository> logger)
        : base(config)
        => (_logger) = (logger)
    ;

    public async Task<LeagueTable> GetLeagueTable(string competitionCode)
    {
        var uri = MatchesUri.Replace("<COMPETITION_CODE>", competitionCode.ToUpper());
        return await base.GetResource<LeagueTable>(uri);
    }
}
