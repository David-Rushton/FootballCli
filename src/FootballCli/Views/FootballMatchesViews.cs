using Spectre.Console.Rendering;
using Humanizer;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Dr.FootballCli.Views;

public static class FootballMatchesViews
{
    /// <summary>
    /// Converts matches into tables.
    /// </summary>
    /// <returns>One table per matchday provided.</returns>
    public static IEnumerable<Table> ToMatchDayTables(this FootballMatches matches)
    {
        var matchDays = (from match in matches.Matches select match.KickOff.Date).Distinct();

        foreach (var matchDay in matchDays)
        {
            var table = new Table()
                .Title(HumaniseDate(matchDay))
                .MarkdownBorder()
                .AddColumn("Home")
                .AddColumn("Away")
                .AddColumn("Status");

            var matchDayMatches = matches
                .Matches
                .Where(match => match.KickOff.Date == matchDay)
                .ToList();

            for (var i = 0; i < matchDayMatches.Count; i++)
            {
                if (i != 0 && i % 4 == 0 && matchDayMatches.Count >= 6)
                    table.AddEmptyRow();

                table.AddRow(GetRow(matchDayMatches[i]));
            }

            yield return table;
        }

        static string HumaniseDate(DateTime dateTime) =>
            (dateTime.Date - DateTime.UtcNow.Date).TotalDays switch
            {
                -1 => "Yesterday",
                0 => "Today",
                1 => "Tommorrow",
                // > 1 and < 7 => dateTime.ToString("ddd"),
                _ => dateTime.ToString("MMM d")

            };

        static IRenderable[] GetRow(FootballMatch match) =>
            new[]
            {
                new Markup(match.HomeTeam.ShortName),
                new Markup(match.AwayTeam.ShortName),
                GetMatchStatus(match)
            };

        static Markup GetMatchStatus(FootballMatch match)
        {
            // Status various depending on match state.
            // We highlight cancelled and postponed games, using the standard convention of C and P.
            // For all other future matches we show the kickoff time.  As there is one table per
            // matchday we do not need to include date.
            // We show the final score for completed matches.
            // For games in progress we show....?

            var status = match.StatusCode switch
            {
                FootballMatchStatusCode.Cancelled       => $"C",
                FootballMatchStatusCode.Postponed       => $"P",
                FootballMatchStatusCode.Suspended       => $"S",
                FootballMatchStatusCode.Scheduled       => $"{match.KickOff.ToLocalTime():HH:mm}",
                FootballMatchStatusCode.Timed           => $"{match.KickOff.ToLocalTime():HH:mm}",
                FootballMatchStatusCode.InPlay          => $"{GetScore()}",
                FootballMatchStatusCode.Paused          => $"{GetPausedStatus()} {GetScore()}",
                FootballMatchStatusCode.Live            => $"{GetScore()}",
                FootballMatchStatusCode.ExtraTime       => $"ET {GetScore()}",
                FootballMatchStatusCode.PenaltyShootout => $"PEN {GetScore()}",
                FootballMatchStatusCode.Finished        => $"FT {GetScore()}",
                FootballMatchStatusCode.Awarded         => $"{GetScore()} (Awarded)",
                _ => throw new NotSupportedException(
                    $"Match status: {match.StatusCode} not supported.\nPlease raise an issue: https://github.com/David-Rushton/FootballCli/issues")
            };

            return new Markup(status.EscapeMarkup());

            string GetPausedStatus() =>
                (DateTime.UtcNow - match.KickOff).TotalMinutes switch
                {
                    >= 120 => "Penalties to follow",
                    >= 105 => "ET HT",
                    _ => "HT"
                };

            string GetScore()
            {
                var home = match.Score.ExtraTime.Home
                    ?? match.Score.FullTime.Home
                    ?? match.Score.HalfTime.Home
                    ?? 0;
                var away = match.Score.ExtraTime.Away
                    ?? match.Score.FullTime.Away
                    ?? match.Score.HalfTime.Away
                    ?? 0;

                return match.Score.Penalties.Home != null && match.Score.Penalties.Away != null
                    ? $"{home}-{away} ({match.Score.Penalties.Home}-{match.Score.Penalties.Away})"
                    : $"{home}-{away}";
            }
        }
    }
}
