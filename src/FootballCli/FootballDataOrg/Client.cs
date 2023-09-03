using System.Text.RegularExpressions;

namespace Dr.FootballCli.FootballDataOrg;

/// <summary>
/// Thrown if a download from <see href="https://www.football-data.org">football-data.org</see> fails.
/// </summary>
public class ClientDownloadFailedException(
    string message, Exception? innerException = null) : Exception(message, innerException)
{

}

/// <summary>
/// A simple HTTP client for retrieving data from
/// <see href="https://www.football-data.org">football-data.org</see>.
/// </summary>
///
public class Client
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public Client(IOptions<ApiOptions> options)
    {
        _client = new HttpClient { BaseAddress = GetBaseUri() };
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.ApiKey);

        Uri GetBaseUri() =>
            options.Value.BaseUrl.EndsWith("/")
                ? new Uri(options.Value.BaseUrl)
                : new Uri($"{options.Value.BaseUrl}/");
    }

    public async Task<FootballCompetitions> GetCompetitions() =>
        await GetRequest<FootballCompetitions>("/v4/competitions");

    public async Task<FootballMatches> GetMatches(string competitionCode, DateTime from, DateTime until)
    {
        var path = $"/v4/competitions/{competitionCode.ToUpper()}/matches?dateFrom={from:yyyy-MM-dd}&dateTo={until:yyyy-MM-dd}";
        var matches = await GetRequest<FootballMatches>(path);

        return matches;
    }

    private async Task<T> GetRequest<T>(string path)
    {
        try
        {
            // TODO: Add Polly retry policy?
            var response = await _client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new ClientDownloadFailedException(
                    $"Unable to download content from: {path}.  Status: {response.StatusCode}.  Please check your request and try again.");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

            if (result is null)
                throw new ClientDownloadFailedException(
                    "Cannot download content.  The server responded but the response was empty.");

            return result;
        }
        catch (ClientDownloadFailedException)
        {
            // Rethrow.  This exposed the stack trace to the command, which will print it out if in
            // verbose mode.
            throw;
        }
    }
}
