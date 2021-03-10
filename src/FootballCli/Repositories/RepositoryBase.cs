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
