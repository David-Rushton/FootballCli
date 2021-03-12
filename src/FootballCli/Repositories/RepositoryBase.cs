using FootballCli.Config;
using FootballCli.Commands.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
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
        readonly protected HttpClient _client = new();

        readonly protected SourceConfig _config;


        public RepositoryBase(IOptions<SourceConfig> config)
        {
            _config = config.Value;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("X-Auth-Token", _config!.ApiKey);
        }
    }
}


/*
HTTP error codes returned
400 Bad Request	Your request was malformed. Most likely the value of a Filter was not set according to the Data Type that is expected.
403 Restricted Resource	You tried to access a resource that exists, but is not available to you. This can be due to the following reasons:
the resource is only available to authenticated clients.

the resource is only available to clients with a paid subscription.

the resource is not available in the API version you are using.

404 Not Found	You tried to access a resource that doesn't exist
429 Too Many Requests	You exceeded your API request quota. See Request-Throttling for more information.
*/
