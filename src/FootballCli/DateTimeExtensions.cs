namespace Dr.FootballCli;

public static class DateTimeExtensions
{
    public static int SecondsSinceUtcNow(this DateTime dateTime) =>
        (DateTime.UtcNow - dateTime).Seconds
    ;
}
