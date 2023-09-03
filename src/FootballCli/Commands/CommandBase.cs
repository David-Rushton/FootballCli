using Dr.FootballCli.Extensions;

namespace Dr.FootballCli.Commands;

/// <summary>
/// Provides code common to all football commands.
/// </summary>
public abstract class CommandBase<T> : AsyncCommand<T> where T : CommonSettings
{
    public override async Task<int> ExecuteAsync(CommandContext context, T settings)
    {
        try
        {
            return settings.ResultsToJson
                ? await JsonHandler(context, settings)
                : await ObjectHandler(context, settings);
        }
        catch (Exception e)
        {
            if (settings.ResultsToJson)
            {
                AnsiConsole.WriteLine(
                    settings.VerboseModeEnabled
                        ? new { error = e.Message, stackTrace = e.StackTrace }.ToJson()
                        : new { error = e.Message }.ToJson());

                return 1;
            }

            if (settings.VerboseModeEnabled && e.StackTrace is not null)
                AnsiConsole.WriteLine(e.StackTrace);

            // When we are not in json mode we rethrow.
            // The exception is then caught, formatted and printed by Spectre.Console.
            Environment.ExitCode = 1;
            throw;
        }
    }

    /// <summary>
    /// Handles the request if --json is provided.
    /// </summary>
    /// <returns>Application exit code</returns>
    public abstract Task<int> JsonHandler(CommandContext context, T settings);

    /// <summary>
    /// Handles the request when --json is not present.
    /// </summary>
    /// <returns>Application exit code</returns>
    public abstract Task<int> ObjectHandler(CommandContext context, T settings);

}
