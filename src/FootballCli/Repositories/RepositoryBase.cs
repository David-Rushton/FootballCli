using FootballCli.Config;
using FootballCli.Commands.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace FootballCli.Repositories
{
    public class RepositoryBase
    {
        const string _fallbackExceptionMessage = "Something went wrong.  Unable to download football data.  :(";

        const string BaseUri = "http://api.football-data.org";

        readonly HttpClient _client = new();

        readonly SourceConfig _config;


        public RepositoryBase(IOptions<SourceConfig> config)
        {
            _config = config.Value;
            _client.BaseAddress = new System.Uri(BaseUri);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("X-Auth-Token", _config!.ApiKey);
        }


        protected async Task<T> GetResource<T>(string uri)
        {
            try
            {
                var stream = _client.GetStreamAsync(uri);
                var resource = await JsonSerializer.DeserializeAsync<T>(await stream);

                if(resource is null)
                    throw new Exception("Could not retrieve football data :(");

                return resource;
            }
            catch(HttpRequestException e)
            {
                throw new Exception(GetHttpRequestExceptionMessage(e), e);
            }
            catch(TaskCanceledException)
            {
                // todo: we don't current support cancellation tokens
                throw new Exception("Request cancelled");
            }
            catch(Exception e)
            {
                throw new Exception(_fallbackExceptionMessage, e);
            }
        }


        /// <summary>
        /// Maps server exceptions to client friendly messages.
        /// </summary>
        /// <param name="exception">Server exception</param>
        /// <returns>string</returns>
        private string GetHttpRequestExceptionMessage(HttpRequestException exception)
        {
            if(exception.StatusCode is null)
                return _fallbackExceptionMessage;


            // Server error messages:
            // www.football-data.org/documentation/api#http-errors
            //   400 - Bad/malformed request.  Most likely an unsupported filter value.
            //   403 - Restricted .  Upgrade subscription to access.
            //   404 - Not found.
            //   429 - Rate limit exceeded.
            return ((int)exception.StatusCode) switch
            {
                400 => _fallbackExceptionMessage,
                403 => "Please upgrade your //football-data.org subscription to access this resource",
                404 => _fallbackExceptionMessage,
                429 => "Rate limit exceeded.  Please try again later.",
                _   => _fallbackExceptionMessage,
            };
        }
    }
}
