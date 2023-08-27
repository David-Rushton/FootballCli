namespace Dr.FootballCli.Model;

public readonly record struct LeaguePosition(
    int Position,
    FootballTeam Team,
    int PlayedGames,
    string Form,
    int Won,
    int Drawn,
    int Lost,
    int Points,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference);
