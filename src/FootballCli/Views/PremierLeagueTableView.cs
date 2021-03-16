using FootballCli.Config;
using FootballCli.Model;
using Microsoft.Extensions.Options;
using System;


namespace FootballCli.Views
{
    public class PremierLeagueTableView : TableViewBase
    {
        public PremierLeagueTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }


        public override void RenderTable(LeagueTable leagueTable) =>
            base.RenderTable(leagueTable, new[] { 4, 17 }, false)
        ;
    }
}
