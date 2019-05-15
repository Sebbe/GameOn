using System.Collections.Generic;
using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    /// <summary>
    /// Defines a readonly match service
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Returns a page of pool matches
        /// </summary>
        /// <param name="args">A dynamic object of Arguments. PageSize = The size of the page to return. If missing, will return all items</param>
        Task<IList<Match>> GetMatches(dynamic args);

        /// <summary>
        /// Returns a page of pool matches for a player
        /// </summary>
        /// <param name="playerId">The player ID</param>
        /// <param name="args">A dynamic object of Arguments. PageSize = The size of the page to return. If missing, will return all items</param>
        Task<IList<Match>> GetMatches(int playerId, dynamic args);
    }
}