using System;


namespace FootballCli.Views
{
    public class TableViewFactory
    {
        public TableViewBase GetTableView(string competitionCode) =>
            competitionCode.ToLower() switch
            {
                "pl"  => new PremierLeagueTableView(),
                _     => new DefaultTableView()
            }
        ;
    }
}
