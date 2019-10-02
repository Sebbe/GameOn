using System.Collections.Generic;
using GameOn.Web.Entities;

namespace GameOn.Web.Models
{
    /// <summary>
    /// Model to represent the list of matches that a Player has played
    /// </summary>
    public class TeamMatchListModel
    {
        /// <summary>
        /// The Team for these matches
        /// </summary>
        public Team Team { get; set; }

        /// <summary>
        /// A summary of match statistics for this.Player
        /// </summary>
        public TeamMatchesSummary MatchesSummary { get; set; }

        /// <summary>
        /// A list of Matches played by this.Player
        /// </summary>
        public IList<Match> Matches { get; set; }
    }
}