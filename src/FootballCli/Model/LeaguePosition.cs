using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class LeaguePosition
    {
        [JsonPropertyName("position")]
        public int Position { get; init; }

        [JsonPropertyName("team")]
        public FootballTeam Team { get; init; } = new();

        [JsonPropertyName("playedGames")]
        public int PlayedGames { get; init; }

        [JsonPropertyName("form")]
        public string Form { get; init; } = string.Empty;

        [JsonPropertyName("won")]
        public int Won { get; init; }

        [JsonPropertyName("draw")]
        public int Drawn { get; init; }

        [JsonPropertyName("lost")]
        public int Lost { get; init; }

        [JsonPropertyName("points")]
        public int Points { get; init; }

        [JsonPropertyName("goalsFor")]
        public int GoalsFor { get; init; }

        [JsonPropertyName("goalsAgainst")]
        public int GoalsAgainst { get; init; }

        [JsonPropertyName("goalDifference")]
        public int GoalDifference { get; init; }
    }
}
