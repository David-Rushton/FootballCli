using Microsoft.Extensions.Logging;
using Dr.FootballCli.Commands.Settings;
using Dr.FootballCli.Repositories;
using Dr.FootballCli.Views;

namespace Dr.FootballCli.Commands
{
    public class TableCommand : AsyncCommand<TableSettings>
    {
        readonly ILogger<TableCommand> _logger;

        readonly LeagueRepository _leagueRepository;

        readonly CompetitionRepository _competitionRepository;

        readonly TableViewFactory _tableViewFactory;


        public TableCommand(
            ILogger<TableCommand> logger,
            LeagueRepository leagueRepository,
            CompetitionRepository competitionRepository,
            TableViewFactory tableViewFactory
        )
        {
            _logger = logger;
            _leagueRepository = leagueRepository;
            _competitionRepository = competitionRepository;
            _tableViewFactory = tableViewFactory;
        }


        public override async Task<int> ExecuteAsync(CommandContext context, TableSettings settings)
        {
            var competitionCode = settings.CompetitionCode;
            var competitions = await _competitionRepository.GetCompetitions();
            var IsValidCompetitionCode = competitions.IsValidCompetitionCode(competitionCode);
            var tableView = _tableViewFactory.GetTableView(competitionCode);

            if(IsValidCompetitionCode)
            {
                tableView.RenderTable(await _leagueRepository.GetLeagueTable(competitionCode), settings.FollowLive);
                return 0;
            }


            throw new InvalidCompetitionCodeException(competitionCode);
        }
    }
}
