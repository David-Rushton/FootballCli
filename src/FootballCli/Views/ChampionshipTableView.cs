using Microsoft.Extensions.Options;
using System;
using Dr.FootballCli.Config;
using Dr.FootballCli.Model;


namespace Dr.FootballCli.Views
{
    public class ChampionshipTableView : TableViewBase
    {
        public ChampionshipTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }


        public override void RenderTable(LeagueTable leagueTable, bool showFullTable) =>
            base.RenderTable(leagueTable, new[] { 2, 6, 21 }, showFullTable)
        ;
    }
}
