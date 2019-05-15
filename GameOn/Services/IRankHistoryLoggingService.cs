using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    /// <summary>
    /// Defines a service for logging a Player's Ranking history
    /// </summary>
    public interface IRankHistoryLoggingService
    {
        /// <summary>
        /// Saves a ranking history record for a player
        /// </summary>
        Task SaveRankHistory(Player player);
    }


}
