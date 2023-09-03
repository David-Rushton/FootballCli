namespace Dr.FootballCli.Model;

public readonly record struct FootballMatchScore(
    string Winner,
    string Duration,
    FootballMatchPeriodScore HalfTime,
    FootballMatchPeriodScore FullTime,
    FootballMatchPeriodScore ExtraTime,
    FootballMatchPeriodScore Penalties)
{
    [Obsolete()]
    public string PrettyPrint()
    {
        var home = ExtraTime.Home
            ?? FullTime.Home
            ?? HalfTime.Home
            ?? 0;
        var away = ExtraTime.Away
            ?? FullTime.Away
            ?? HalfTime.Away
            ?? 0;

        var score = Penalties.Home is not null && Penalties.Away is not null
            ? $"{home}-{away} ({Penalties.Home}-{Penalties.Away})"
            : $"{home}-{away}";

        return score;
    }
}
