
namespace Dr.FootballCli.Commands;

public class CompetitionsCommand(Client client) : CommandBase<CompetitionsCommand.Settings>
{
    public class Settings : CommonSettings
    {

    }

    public override async Task<int> JsonHandler(CommandContext context, Settings settings)
    {
        var competitons = await client.GetCompetitions(settings.VerboseModeEnabled);
        AnsiConsole.WriteLine(competitons.ToJson());
        return 0;
    }

    public override async Task<int> ObjectHandler(CommandContext context, Settings settings)
    {
        var competitons = await client.GetCompetitions(settings.VerboseModeEnabled);
        AnsiConsole.Write(competitons.ToTable());
        return 0;
    }
}
