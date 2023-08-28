using Microsoft.Extensions.Options;
using System;
using Dr.FootballCli.Options;


namespace Dr.FootballCli.Views
{
    public class TableViewFactory
    {
        readonly IOptions<FavouriteTeamConfig> _favouriteTeamConfig;


        public TableViewFactory(IOptions<FavouriteTeamConfig> favouriteTeamConfig) =>
            _favouriteTeamConfig = favouriteTeamConfig
        ;


        public TableViewBase GetTableView(string competitionCode) =>
            competitionCode.ToLower() switch
            {
                "pl"  => new PremierLeagueTableView(_favouriteTeamConfig),
                "elc" => new ChampionshipTableView(_favouriteTeamConfig),
                "cl"  => new GroupAndKnockoutCompetitionTableView(_favouriteTeamConfig),
                "ec"  => new GroupAndKnockoutCompetitionTableView(_favouriteTeamConfig),
                "wc"  => new GroupAndKnockoutCompetitionTableView(_favouriteTeamConfig),
                _     => new DefaultTableView(_favouriteTeamConfig)
            }
        ;
    }
}
