using System.Diagnostics;
using TimeWindow = (System.DateTime from, System.DateTime until);

namespace Dr.FootballCli.Commands;

public enum MatchesDuring
{
    Last7,
    Yesterday,
    Today,
    Tomorrow,
    Next7
}

public class MatchesCommand(Client client) : CommandBase<MatchesCommand.Settings>
{
    public class Settings : CommonSettings
    {
        [CommandArgument(position: 0, template: "<competition-code>")]
        [Description("See competitons for a full list of supported codes.")]
        public required string CompetitionCode { get; init; }

        [CommandOption(template: "-f|--from")]
        [DefaultValue("0")]
        [Description("Supports ISO-8601 date time strings.\nSupports relative offsets from today, including -1, +1, 1, ...etc")]
        public string From { get; init; } = "0";

        [CommandOption(template: "-u|--until")]
        [DefaultValue("0")]
        [Description("Supports ISO-8601 date time strings.\nSupports relative offsets from today, including -1 (yesterday), 1/+1 (tomorrow), etc")]
        public string Until { get; init; } = "0";


        public override ValidationResult Validate()
        {
            var errorMessage = new StringBuilder();

            if (!DateTime.TryParse(From, out _) && !int.TryParse(From, out _))
                errorMessage.AppendLine($"  - --from value not supported: {From}.\n    See matches --help for supported values.");

            if (!DateTime.TryParse(Until, out _) && !int.TryParse(Until, out _))
                errorMessage.AppendLine($"  - --until value not supported: {Until}.\n    See matches --help for supported values.");

            return errorMessage.Length > 0
                ? ValidationResult.Error($"The request cannot be completed.\nPlease check and correct these input problems:\n\n{errorMessage.ToString()}")
                : ValidationResult.Success();
        }
    }

    public override async Task<int> JsonHandler(CommandContext context, Settings settings)
    {
        var timeWindow = GetTimeWindow(settings);
        var matches = await client.GetMatches(settings.CompetitionCode, timeWindow.from, timeWindow.until);

        AnsiConsole.WriteLine(matches.ToJson());

        return 0;
    }

    public override async Task<int> ObjectHandler(CommandContext context, Settings settings)
    {
        var timeWindow = GetTimeWindow(settings);
        var matches = await client.GetMatches(settings.CompetitionCode, timeWindow.from, timeWindow.until);

        foreach (var table in matches.ToMatchDayTables())
        {
            AnsiConsole.Write(table);
        }

        return 0;
    }

    private TimeWindow GetTimeWindow(Settings settings)
    {
        // User can provide from & until as date time strings of numeric offsets.
        // If a numeric offset it is the number of days to +/- from today.
        //   Example: --from 2023-09-03 is converted to a DateTime of September 3rd, 2023.
        //   Example: --from -1 is converted to a DateTime of yesterday.
        //   Example: --from +1 is converted to a DateTime of today, at midnight.
        //   Example: --from 1 is converted to a DateTime of today, at midnight.
        // Settings.Validate() ensures from/until are always one of these two expected types.

        var from = DateTime.MinValue;
        var until = DateTime.MaxValue;

        if (int.TryParse(settings.From, out var fromOffset))
            from = DateTime.UtcNow.AddDays(fromOffset);
        else
            DateTime.TryParse(settings.From, out from);

        if (int.TryParse(settings.Until, out var untilOffset))
            until = DateTime.UtcNow.AddDays(untilOffset);
        else
            DateTime.TryParse(settings.Until, out until);

        Debug.Assert(from != DateTime.MinValue, "Failed to parse matches --from.");
        Debug.Assert(until != DateTime.MinValue, "Failed to parse matches --until.");

        // Be nice and allow for the user mixing from/until values.
        return (
            from <= until ? from : until,
            from <= until ? until : from);
    }
}
