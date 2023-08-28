using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dr.FootballCli.Options;
using Dr.FootballCli.Model;

namespace Dr.FootballCli.Repositories;

public class CompetitionRepository : RepositoryBase
{
    const string CompetitionsUri = "http://api.football-data.org/v2/competitions/?plan=TIER_ONE";

    readonly ILogger<CompetitionRepository> _logger;

    public CompetitionRepository(IOptions<ApiOptions> config, ILogger<CompetitionRepository> logger)
        : base(config)
        => (_logger) = (logger)
    ;

    public async Task<FootballCompetitions> GetCompetitions() =>
        await base.GetResource<FootballCompetitions>(CompetitionsUri)
    ;
}
