namespace Dr.FootballCli.Model;

public readonly record struct LeagueTable(
    FootballCompetition Competition,
    FootballSeason Season,
    List<LeagueStanding> Standings);
