using Microsoft.Extensions.Options;
using System;
using Dr.FootballCli.Options;
using Dr.FootballCli.Model;


namespace Dr.FootballCli.Views
{
    public class GroupAndKnockoutCompetitionTableView : TableViewBase
    {
        public GroupAndKnockoutCompetitionTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }


        public override void RenderTable(LeagueTable leagueTable, bool showFullTable) =>
            base.RenderTable(leagueTable, new[] { 2 }, showFullTable)
        ;
    }
}
