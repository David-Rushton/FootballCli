using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;


namespace FootballCli.Commands.Settings
{
    public class TableSettings : CommandSettings
    {
        [CommandArgument(0, "<COMPETITION_CODE>")]
        public string CompetitionCode { get; set; } = string.Empty;

        [CommandOption("-f|--full-table")]
        [Description("Show the full table")]
        public bool FollowLive { get; set; }
    }
}
