# Football Cli

Folllow the match in your terminal.

## Build

This is a .Net core console app.  You can build and run using standing .Net tooling.

## Configuration

This app is powered by [//football-data.org](https://www.football-data.org/).  A fantastic restful
source of footballing data.  You'll need an [API token](https://www.football-data.org/client/register).
Once registered add to your local `appsettings.json` file.

```
{
    "source": {
        "apiKey": "<api-token-here>"
    }
}
```

### View Full Exception Details

This project uses the excellent [Spectre.Console](https://spectresystems.github.io/spectre.console/)
to render to the console.  Spectre.Console pretty prints exceptions, which is great for users but
terrible for devlopers.  To view full exception messages add this env var:

```powershell
$ENV:DOTNET_ENVIRONMENT = "Development"
```

```bash
export DOTNET_ENVIRONMENT=Development
```
