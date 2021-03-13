using System;
using FootballCli.Model;

namespace FootballCli.Views
{
    public class PremierLeagueTableView : TableViewBase
    {
        public override void RenderTable(LeagueTable leagueTable) =>
            base.RenderTable(leagueTable, new[] { 4, 17 })
        ;
    }
}
