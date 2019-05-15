using GameOn.Web.Entities;

namespace GameOn.Web.Models
{
    /// <summary>
    /// Used to display a summary of all of the matches that a Player has played, for all time.
    /// </summary>
    public class PlayerMatchesSummary
    {
        /// <summary>
        /// The Player for this summary
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// The number of matches this.Player has played
        /// </summary>
        public int Played { get; set; }

        /// <summary>
        /// The number of matches this.Player has won
        /// </summary>
        public int Won { get; set; }

        /// <summary>
        /// The number of matches this.Player has lost
        /// </summary>
        public int Lost
        {
            get { return Played - Won; }
        }

        /// <summary>
        /// The highest ranking that this player has held
        /// </summary>
        public int? HighestRanking { get; set; }

        /// <summary>
        /// The lowest ranking that this player has held
        /// </summary>
        public int? LowestRanking { get; set; }
    }
}