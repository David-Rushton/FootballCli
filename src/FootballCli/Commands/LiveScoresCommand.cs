using FootballCli;
using FootballCli.Config;
using FootballCli.Commands.Settings;
using FootballCli.Model;
using FootballCli.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace FootballCli.Commands
{
    public class LiveScoresCommand : AsyncCommand<LiveScoresSettings>
    {
        const int LiveRefreshIntervalInMilliseconds = 10000;

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
            await ThrowIfCompetitionCodeIsInvalid(settings.CompetitionCode);

            if(settings.FollowLive)
                await FollowLive();
            else
                await RenderScores(settings.CompetitionCode);


            return 0;


            async Task FollowLive()
            {
                ConfigureConsoleForFollowLive();

                while(true)
                    await UpdateDisplay();
            }

            async Task UpdateDisplay()
            {
                await RenderScores(settings.CompetitionCode);
                Thread.Sleep(LiveRefreshIntervalInMilliseconds);
                Console.SetCursorPosition(0, 0);
            }
        }


        private async Task ThrowIfCompetitionCodeIsInvalid(string competitionCode)
        {
            var _competitions = await _competitionRepository.GetCompetitions();

            if( ! _competitions.IsValidCompetitionCode(competitionCode) )
                throw new InvalidCompetitionCodeException(competitionCode);
        }

        private void ConfigureConsoleForFollowLive()
        {
            Console.CancelKeyPress += (o, e) => Console.CursorVisible = true;
            Console.CursorVisible = false;
            Console.Clear();
        }

        private async Task RenderScores(string competitionCode)
        {
            var matches = await _matchRepository.GetLiveMatches(competitionCode);
            var lastUpdated = matches.Matches.Max(m => m.LastUpdate);
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("Home").RightAligned())
                .AddColumn(new TableColumn("Score").Centered())
                .AddColumn("Away")
            ;

            foreach(var match in matches.Matches)
            {
                var matchRevised = (match.Revision.Number > 0 && match.Revision.UtcDateTime.SecondsSinceUtcNow() is >=0 and < 120);

                // todo: leading space is required.  add comment or refactor.
                var style = matchRevised ? " slowblink" : string.Empty;
                var colour = PrettyPrintColour(match.StatusCode);
                table.AddRow
                (
                    $"[{colour}{style}]{match.HomeTeam.Name}[/]",
                    $"[{colour}{style}]{match.PrettyPrintState()}[/]",
                    $"[{colour}{style}]{match.AwayTeam.Name}[/]"
                );
            }

            AnsiConsole.Render(table);
            AnsiConsole.MarkupLine($"[bold blue]Last updated:[/] [blue]{lastUpdated.ToLocalTime().ToShortTimeString()}[/]");
        }

        private string PrettyPrintColour(FootballMatchStatusCode statusCode) =>
            statusCode switch
            {
                FootballMatchStatusCode.Scheduled => "lightslategrey",
                FootballMatchStatusCode.Live      => "lightskyblue1",
                FootballMatchStatusCode.InPlay    => "lightskyblue1",
                FootballMatchStatusCode.Paused    => "grey58",
                FootballMatchStatusCode.Finished  => "bold yellow",
                FootballMatchStatusCode.Postponed => "red3_1",
                FootballMatchStatusCode.Suspended => "red3_1",
                FootballMatchStatusCode.Cancelled => "red3_1",
                _                                 => throw new Exception($"Match status code not supported: {statusCode}")
            }
        ;
    }
}
