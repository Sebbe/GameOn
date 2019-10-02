using GameOn.Web.Entities;

namespace GameOn.Web.Models
{
    /// <summary>
    /// A model that represents the entities for the Play view
    /// </summary>
    public class PlayModel
    {
        /// <summary>
        /// Team 1
        /// </summary>
        public Team Team1 { get; set; }

        /// <summary>
        /// Team 2
        /// </summary>
        public Team Team2 { get; set; }
    }
}