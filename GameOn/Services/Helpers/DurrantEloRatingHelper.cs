using System;
using GameOn.Web.Entities;

namespace GameOn.Web.Services.Helpers
{
    /// <summary>
    /// This class is based on Luke Durrant's implementation of the ELO Rating algorythm.
    /// http://lukedurrant.com/2010/11/c-elo-rating-class-used-on-facemash-as-seen-in-the-social-network-movie/
    /// It provides a method to update the ranks of the players of a match, based on who won and their current ranking.
    /// </summary>
    /// <remarks>This implementation does not allow drawn matches. There must be a winner for ranking to take place.</remarks>
    public class DurrantEloRatingHelper : IRatingHelper
    {
        /// <summary>
        /// Updates the ranks of players of a match
        /// </summary>
        /// <param name="match">The Match to update</param>
        public void UpdateMatchPlayersRanks(Match match)
        {
            double currentRating1 = match.PlayerOne.CurrentRank;
            double currentRating2 = match.PlayerTwo.CurrentRank;

            double finalResult1;
            double finalResult2;

            double e;

            if (match.WinnerPlayerId == match.PlayerOneId)
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / 400))) * 120);
                finalResult1 = currentRating1 + e;
                finalResult2 = currentRating2 - e;
            }
            else
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / 400))) * 120);
                finalResult1 = currentRating1 - e;
                finalResult2 = currentRating2 + e;
            }

            match.PlayerOne.CurrentRank = (int)finalResult1;
            match.PlayerTwo.CurrentRank = (int)finalResult2;
        }
    }
}