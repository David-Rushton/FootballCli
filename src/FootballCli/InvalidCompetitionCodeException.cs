using System;


namespace FootballCli
{
    public class InvalidCompetitionCodeException : Exception
    {
        public InvalidCompetitionCodeException(string competitionCode)
            : base($"Competition code not supported: {competitionCode}.\nSee: FootballCli competitions")
        { }
    }
}
