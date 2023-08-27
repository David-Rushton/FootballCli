namespace Dr.FootballCli.Model;

public readonly record struct FootballMatchScore(
    string Winner,
    string Duration,
    FootballMatchPeriodScore FullTime,
    FootballMatchPeriodScore HalfTime,
    FootballMatchPeriodScore ExtraTime,
    FootballMatchPeriodScore Penalties);
