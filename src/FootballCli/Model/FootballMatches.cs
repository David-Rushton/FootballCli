using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public class FootballMatches
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("matches")]
        public List<FootballMatch> Matches { get; set; } = new();


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
            ).Count()
        ;
    }
}
