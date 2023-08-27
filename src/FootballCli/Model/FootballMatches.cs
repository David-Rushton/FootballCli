namespace Dr.FootballCli.Model;

public readonly record struct FootballMatches(
    int Count,
    List<FootballMatch> Matches)
{
    public int GamesRemainingToday() =>
        (
            from match in Matches
            where
                (
                       match.StatusCode == FootballMatchStatusCode.Scheduled
                    || match.StatusCode == FootballMatchStatusCode.Live
                    || match.StatusCode == FootballMatchStatusCode.InPlay
                    || match.StatusCode == FootballMatchStatusCode.Paused
                )
                && match.KickOff.Date == DateTime.UtcNow
            select 1
        ).Count();
}
