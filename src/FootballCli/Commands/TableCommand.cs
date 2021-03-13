using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using FootballCli.Repositories;
using FootballCli.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Linq;
using System.Threading.Tasks;


namespace FootballCli.Commands
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
                tableView.RenderTable(await _leagueRepository.GetLeagueTable(competitionCode));
                return 0;
            }


            throw new InvalidCompetitionCodeException(competitionCode);
        }
    }
}
