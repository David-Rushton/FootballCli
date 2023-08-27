using System.Runtime.InteropServices.JavaScript;

namespace Dr.FootballCli.Model;

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

public readonly record struct FootballMatch(
    int Id,
    [property: JsonPropertyName("utcDate")]
    DateTime KickOff,
    DateTime LastUpdate,
    string Status,
    int MatchDay,
    FootballTeam HomeTeam,
    FootballTeam AwayTeam,
    FootballMatchScore Score)
{
    public (int Number, DateTime UtcDateTime) Revision { get; } = (0, DateTime.UtcNow);

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
            "CANCELED"  => FootballMatchStatusCode.Cancelled,
            _           => throw new Exception($"Football match status code not supported: {Status}")
        };

    public string PrettyPrintState() =>
        StatusCode switch
        {
            FootballMatchStatusCode.Scheduled => KickOff.ToString("HH:mm"),
            FootballMatchStatusCode.Live      => PrettyPrintScore(),
            FootballMatchStatusCode.InPlay    => PrettyPrintScore(),
            FootballMatchStatusCode.Paused    => $"{PrettyPrintScore()} (HT)",
            FootballMatchStatusCode.Finished  => $"{PrettyPrintScore()} (FT)",
            FootballMatchStatusCode.Postponed => "Postponed",
            FootballMatchStatusCode.Suspended => "Suspended",
            FootballMatchStatusCode.Cancelled => "Cancelled",
            _                                 => throw new Exception($"Match status code not supported: {StatusCode}")
        };

    public string PrettyPrintScore() =>
        $"{Score.FullTime.HomeTeam} - {Score.FullTime.AwayTeam}";
}
