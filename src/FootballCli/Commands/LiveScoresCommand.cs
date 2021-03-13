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
            await ThrowIfCompetitionCodeInvalid(settings.CompetitionCode);

            while(true)
            {
                await RenderScores();

                if( ! settings.FollowLive )
                    return 0;

                Thread.Sleep(LiveRefreshIntervalInMilliseconds);
            }


            async Task ThrowIfCompetitionCodeInvalid(string competitionCode)
            {
                if( ! await IsCompetitionCodeValid(competitionCode) )
                    throw new InvalidCompetitionCodeException(competitionCode);
            }

            async Task RenderScores()
            {
                var matches = await _matchRepository.GetLiveMatches(settings.CompetitionCode);
                var lastUpdated = matches.Matches.Max(m => m.LastUpdate);
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("Home").RightAligned())
                    .AddColumn(new TableColumn("Score").Centered())
                    .AddColumn("Away")
                ;

                foreach(var match in matches.Matches)
                {
                    var colour = PrettyPrintColour(match.StatusCode);
                    table.AddRow
                    (
                        $"[{colour}]{match.HomeTeam.Name}[/]",
                        $"[{colour}]{match.Score.FullTime.HomeTeam} - {match.Score.FullTime.AwayTeam}[/]",
                        $"[{colour}]{match.AwayTeam.Name}[/]"
                    );
                }

                AnsiConsole.Render(table);
                AnsiConsole.MarkupLine($"[bold blue]Last updated:[/] [blue]{lastUpdated.ToLocalTime()}[/]");
            }

            string PrettyPrintColour(FootballMatchStatusCode statusCode) =>
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


        private async Task<bool> IsCompetitionCodeValid(string competitionCode)
        {
            var competitionCodes =
                from competition in (await _competitionRepository.GetCompetitions()).Items
                select competition.Code.ToLower()
            ;


            return competitionCodes.Contains(competitionCode.ToLower());
        }


        public class InvalidCompetitionCodeException : Exception
        {
            public InvalidCompetitionCodeException(string competitionCode)
                : base($"Competition code not supported: {competitionCode}.\nSee: FootballCli competitions")
            { }
        }
    }
}
