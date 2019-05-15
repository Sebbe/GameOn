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
        public Task SaveRankHistory(Player player)
        {
            var rankHistory = new RankHistory { Player = player, Rank = player.CurrentRank, Date = DateTime.Now };
            _gameOnContext.RankHistory.Add(rankHistory);
            return _gameOnContext.SaveChangesAsync();
        }
    }
}
