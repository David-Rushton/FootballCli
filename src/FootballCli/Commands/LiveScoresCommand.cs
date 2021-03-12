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
    public class LiveScoresCommand : AsyncCommand<LiveScoresSettings>
    {
        readonly ILogger<LiveScoresCommand> _logger;

        readonly CompetitionRepository _competitionRepository;

        readonly MatchRepository _matchRepository;


        public LiveScoresCommand(
            ILogger<LiveScoresCommand> logger,
            CompetitionRepository competitionRepository,
            MatchRepository matchRepository
        ) =>
            (_logger, _competitionRepository, _matchRepository) = (logger, competitionRepository, matchRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context, LiveScoresSettings settings)
        {
            var competition =
            (
                from footballCompetition in (await _competitionRepository.GetCompetitions()).Items
                where footballCompetition.Code.ToLower() == settings.CompetitionCode.ToLower()
                select new
                {
                    Code = footballCompetition.Code,
                    CurrentMatchday = footballCompetition.CurrentSeason.CurrentMatchday
                }
            ).FirstOrDefault();


            if(competition is null)
                throw new System.Exception($"Competition code not supported: {settings.CompetitionCode}\nSee: FootballCli competition");





            // ----------------------------------




            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("Home").RightAligned());
            table.AddColumn(new TableColumn("Score").Centered());
            table.AddColumn("Away");

            // bug: matchday can be zero here.
            var matches = await _matchRepository.GetMatches(settings.CompetitionCode, settings.Matchday);
            foreach(var match in matches.Matches)
            {
                var colour = PrettyPrintColour(match.Status);
                table.AddRow
                (
                    $"[{colour}]{match.HomeTeam.Name}[/]",
                    $"[{colour}]{match.Score.FullTime.HomeTeam} - {match.Score.FullTime.AwayTeam}[/]",
                    $"[{colour}]{match.AwayTeam.Name}[/]"
                );
            }


            AnsiConsole.Render(table);
            return 0;


            string PrettyPrintColour(string status) => status == "FINISHED" ? "yellow" : "white";
        }


        private async Task<bool> IsCompetitionCodeValid(string competitionCode)
        {
            var competitionCodes =
                from competition in (await _competitionRepository.GetCompetitions()).Items
                select competition.Code.ToLower()
            ;


            return competitionCodes.Contains(competitionCode.ToLower());
        }
    }
}
