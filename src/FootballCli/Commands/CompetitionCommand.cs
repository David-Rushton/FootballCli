using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using FootballCli.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Linq;
using System.Threading.Tasks;


namespace FootballCli.Commands
{
    public class CompetitionCommand : AsyncCommand
    {
        readonly ILogger<CompetitionCommand> _logger;

        readonly CompetitionRepository _competitionRepository;


        public CompetitionCommand(ILogger<CompetitionCommand> logger, CompetitionRepository competitionRepository) =>
            (_logger, _competitionRepository) = (logger, competitionRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var allCompetitions = await _competitionRepository.GetCompetitions();

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Code");
            table.AddColumn("Name");
            table.AddColumn("Current Matchday");

            foreach(var competition in allCompetitions.Items.OrderBy(c => c.Name))
                table.AddRow
                (
                    competition.Code,
                    competition.Name,
                    competition.CurrentSeason.CurrentMatchday.ToString()
                );

            AnsiConsole.Render(table);

            return 0;
        }
    }
}
