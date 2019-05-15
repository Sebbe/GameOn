using GameOn.Web.Entities;

namespace GameOn.Web.Services.Helpers
{
    /// <summary>
    /// Defines a helper for ranking players of match based on who won, and their current respective ranks
    /// </summary>
    public interface IRatingHelper
    {
        /// <summary>
        /// Updates the ranks of players of a match
        /// </summary>
        /// <param name="match">The Match to update</param>
        void UpdateMatchPlayersRanks(Match match);
    }
}