namespace Dr.FootballCli.Extensions;

public static class ObjectExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    /// <summary>
    /// Converts any object to a JSON string.
    /// </summary>
    public static string ToJson(this object obj) =>
        JsonSerializer.Serialize(obj, JsonOptions);
}
