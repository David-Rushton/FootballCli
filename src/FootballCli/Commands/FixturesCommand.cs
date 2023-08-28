using Dr.FootballCli.FootballDataOrg;

namespace Dr.FootballCli.Commands;

public class FixtureSettings : CommandSettings
{
    [CommandArgument(0, "<COMPETITION_CODE>")]
    public string CompetitionCode { get; set; } = string.Empty;
}

public class FixturesCommand(Client client) : AsyncCommand<FixtureSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, FixtureSettings settings)
    {
        var fixtures = await client.GetFixtures(settings.CompetitionCode);

        if (fixtures.Count == 0)
            AnsiConsole.Write("There are no upcoming fixtures.");

        foreach (var match in fixtures.Matches)
        {
            AnsiConsole.WriteLine(match.ToString());
        }

        return 0;
    }
}
