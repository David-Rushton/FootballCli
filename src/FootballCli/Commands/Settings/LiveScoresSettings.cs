using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;


namespace FootballCli.Commands.Settings
{
    public class LiveScoresSettings : CommandSettings
    {
        [CommandOption("-f|--follow-live")]
        [Description("Follow the live action")]
        public bool FollowLive { get; set; }
    }
}
