using System;
using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    /// <summary>
    /// A Ranking history logging service
    /// </summary>
    public class RankHistoryLoggingService: IRankHistoryLoggingService
    {
        private readonly GameOnContext _gameOnContext;

        public RankHistoryLoggingService(GameOnContext gameOnContext)
        {
            _gameOnContext = gameOnContext;
        }

        /// <summary>
        /// Saves a ranking history record for a player
        /// </summary>
        public Task SaveRankHistory(Team team)
        {
            var rankHistory = new RankHistory { Team = team, Rank = team.CurrentRank, Date = DateTime.Now };
            _gameOnContext.RankHistory.Add(rankHistory);
            return _gameOnContext.SaveChangesAsync();
        }
    }
}
