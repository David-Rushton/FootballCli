using FootballCli.Config;
using FootballCli.Model;
using Microsoft.Extensions.Options;
using System;


namespace FootballCli.Views
{
    public class DefaultTableView : TableViewBase
    {
        public DefaultTableView(IOptions<FavouriteTeamConfig> favouriteTeamConfig)
            : base(favouriteTeamConfig)
        { }
    }
}
