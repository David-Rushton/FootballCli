namespace Dr.FootballCli.Model;

public readonly record struct FootballMatches(
    int Count,
    List<FootballMatch> Matches)
{
    public FootballMatches GetFixtures()
    {
        var fixtureStatuses = new List<FootballMatchStatusCode>
        {
            FootballMatchStatusCode.Cancelled,
            FootballMatchStatusCode.Scheduled,
            FootballMatchStatusCode.Postponed,
            FootballMatchStatusCode.Suspended
        };
        var fixtures = Matches
            .Where(m => fixtureStatuses.Contains(m.StatusCode))
            .ToList();

        return new FootballMatches(fixtures.Count, fixtures);
    }

    public FootballMatches GetResults()
    {
        var results = Matches
            .Where(m => m.StatusCode == FootballMatchStatusCode.Finished)
            .ToList();

        return new FootballMatches(results.Count, results);
    }

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
