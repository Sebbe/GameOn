using GameOn.Web.Entities;

namespace GameOn.Web.Models
{
    /// <summary>
    /// A model that represents the entities for the Play view
    /// </summary>
    public class PlayModel
    {
        /// <summary>
        /// Player 1
        /// </summary>
        public Player Player1 { get; set; }

        /// <summary>
        /// Player 2
        /// </summary>
        public Player Player2 { get; set; }
    }
}