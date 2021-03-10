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

        readonly MatchRepository _matchRepository;


        public CompetitionCommand(ILogger<CompetitionCommand> logger, MatchRepository matchRepository) =>
            (_logger, _matchRepository) = (logger, matchRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            var competitions = await FootballCompetitions.Get();

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Code");
            table.AddColumn("Name");
            table.AddColumn("Current Matchday");

            foreach(var competition in competitions.Competitions.OrderBy(c => c.Name))
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
