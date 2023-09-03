using System.Runtime.InteropServices.JavaScript;

namespace Dr.FootballCli.Model;

public enum FootballMatchStatusCode
{
    Scheduled,
    Live,
    InPlay,
    ExtraTime,
    PenaltyShootout,
    Paused,
    Finished,
    Postponed,
    Suspended,
    Timed,
    Cancelled,
    Awarded
}

public enum FootballMatchType
{
    Fixture,
    Result,
    InPlay
}

public readonly record struct FootballMatch(
    int Id,
    [property: JsonPropertyName("utcDate")]
    DateTime KickOff,
    DateTime LastUpdate,
    string Status,
    int MatchDay,
    DateTime LastUpdated,
    FootballTeam HomeTeam,
    FootballTeam AwayTeam,
    FootballMatchScore Score)
{
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
            "TIMED"     => FootballMatchStatusCode.Timed,
            "CANCELED"  => FootballMatchStatusCode.Cancelled,
            _           => throw new Exception($"Football match status code not supported: {Status}")
        };
}
