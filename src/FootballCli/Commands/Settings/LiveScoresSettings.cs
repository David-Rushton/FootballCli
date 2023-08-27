using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;


namespace Dr.FootballCli.Commands.Settings
{
    public class LiveScoresSettings : CommandSettings
    {
        [CommandArgument(0, "<COMPETITION_CODE>")]
        public string CompetitionCode { get; set; } = string.Empty;

        [CommandOption("-f|--follow")]
        [Description("Follow the live action")]
        public bool FollowLive { get; set; }
    }
}
