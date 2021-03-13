using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FootballCli.Model
{
    public enum FootballMatchStatusCode
    {
        Scheduled,
        Live,
        InPlay,
        Paused,
        Finished,
        Postponed,
        Suspended,
        Cancelled
    }


    public class FootballMatch
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("utcDate")]
        public DateTime KickOff { get; init; }

        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdate { get; init; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        // not provided by the data source.
        public FootballMatchStatusCode StatusCode =>
            (Status) switch
            {
                "SCHEDULED" => FootballMatchStatusCode.Scheduled,
                "LIVE"      => FootballMatchStatusCode.Live,
                "IN_PLAY"   => FootballMatchStatusCode.InPlay,
                "PAUSED"    => FootballMatchStatusCode.Paused,
                "FINISHED"  => FootballMatchStatusCode.Finished,
                "POSTPONED" => FootballMatchStatusCode.Postponed,
                "SUSPENDED" => FootballMatchStatusCode.Suspended,
                // not a typo:  [Canceled/Cancelled] Source uses US English, this app uses UK.
                "CANCELED"  => FootballMatchStatusCode.Cancelled,
                _           => throw new Exception($"Football match status code not supported: {Status}")
            }
        ;

        [JsonPropertyName("matchday")]
        public int MatchDay { get; set; }

        [JsonPropertyName("homeTeam")]
        public FootballTeam HomeTeam { get; init; } = new();

        [JsonPropertyName("awayTeam")]
        public FootballTeam AwayTeam { get; init; } = new();

        [JsonPropertyName("score")]
        public FootballMatchScore Score { get; init; } = new();

    }
}
