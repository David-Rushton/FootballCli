using System.Diagnostics;
using System.Text;
using Dr.FootballCli.Model;
using Dr.FootballCli.Options;
using Microsoft.Extensions.Options;

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
        await GetRequest<FootballCompetitions>("/competition");

    public async Task<LeagueTable> GetLeagueTable(string competitionCode) =>
        // TODO: Validate competition code?
        await GetRequest<LeagueTable>($"/competitions/{competitionCode}/standings");

    public async Task<FootballMatches> GetFixtures(string competitionCode) =>
        await GetFixtures(competitionCode, from: DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

    public async Task<FootballMatches> GetFixtures(string competitionCode, DateTime from, DateTime until)
    {
        var path = $"/v4/competitions/{competitionCode}/matches?dateFrom={from:yyyy-MM-dd}&dateTo={until:yyyy-MM-dd}";
        var matches = await GetRequest<FootballMatches>(path);

        return matches.GetFixtures();
    }

    public async Task<FootballMatches> GetResults(string competitionCode) =>
        await GetResults(competitionCode, from: DateTime.UtcNow.AddDays(-7), until: DateTime.UtcNow);

    public async Task<FootballMatches> GetResults(string competitionCode, DateTime from, DateTime until)
    {
        var path = $"/competitions/{competitionCode}/matches?dateFrom={from:yyyy-MM-dd}&dateTo={until:yyyy-MM-dd}";
        var matches = await GetRequest<FootballMatches>(path);

        return matches.GetResults();
    }

    private async Task<T> GetRequest<T>(string path)
    {
        try
        {
            // TODO: Add Polly retry policy?
            var response = await _client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
                throw new ClientDownloadFailedException(
                    $"Unable to download content.  Status: {response.StatusCode}.");

            var result = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), _jsonOptions);
            if (result is null)
                throw new ClientDownloadFailedException(
                    "Cannot download content.  The server responded but the response was empty.");

            return result;
        }
        catch (ClientDownloadFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new ClientDownloadFailedException("Cannot download content.  Request failed.", e);
        }
    }
}
