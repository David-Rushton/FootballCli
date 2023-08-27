using Microsoft.Extensions.Options;
using System;
using Dr.FootballCli.Config;
using Dr.FootballCli.Model;


namespace Dr.FootballCli.Views
{
    public class DefaultTableView : TableViewBase
    {
        public DefaultTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }


        public override void RenderTable(LeagueTable leagueTable, bool showFullTable) =>
            base.RenderTable(leagueTable, seperatorRows: null, showFullTable)
        ;
    }
}
