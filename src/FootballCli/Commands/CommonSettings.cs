using System.ComponentModel;

namespace Dr.FootballCli.Commands;

/// <summary>
/// Provides a setting of flags supported by all football commands.
/// </summary>
public abstract class CommonSettings : CommandSettings
{
    [CommandOption(template: "-v|--verbose")]
    [Description("Verbose mode enables additional logging and full stack traces.")]
    public bool VerboseModeEnabled { get; init; }

    [CommandOption(template: "-j|--json")]
    [Description("Results are formatted using JSON.")]
    public bool ResultsToJson { get; init; }
}
