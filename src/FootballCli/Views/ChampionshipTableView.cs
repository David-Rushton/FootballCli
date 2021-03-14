using FootballCli.Config;
using FootballCli.Model;
using Microsoft.Extensions.Options;
using System;


namespace FootballCli.Views
{
    public class ChampionshipTableView : TableViewBase
    {
        public ChampionshipTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }


        public override void RenderTable(LeagueTable leagueTable) =>
            base.RenderTable(leagueTable, new[] { 2, 6, 21 })
        ;
    }
}
