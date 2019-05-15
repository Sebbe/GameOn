using System.Collections.Generic;
using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    /// <summary>
    /// Defines a Match Result Entry Service
    /// </summary>
    public interface IMatchResultEntryService
    {
        /// <summary>
        /// Adds a new Match, marked as played, and updates the rankings of each player based on who won, and their current rankings
        /// </summary>
        /// <param name="player1">Player 1's ID</param>
        /// <param name="player2">Player 2's ID</param>
        /// <param name="matchSets">The sets played</param>
        /// <returns>A Match entity expressing the results of the match</returns>
        Task<Match> Played(int player1, int player2, IList<MatchSet> matchSets);
    }
}