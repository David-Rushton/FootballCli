namespace Dr.FootballCli.Model;

public readonly record struct FootballMatches(
    FootballResultSet ResultSet,
    List<FootballMatch> Matches);

