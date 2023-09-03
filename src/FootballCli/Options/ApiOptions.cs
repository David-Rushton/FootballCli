using System.ComponentModel.DataAnnotations;

namespace Dr.FootballCli.Options;

public class ApiOptions
{
    /// <summary>
    /// API key from <see href="https://www.football-data.org">www.football-data.org</see>.
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// API root uri for <see href="https://www.football-data.org">www.football-data.org</see>.
    /// </summary>
    public required string BaseUrl { get; init; } = "https://api.football-data.org/";
}
