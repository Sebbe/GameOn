using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    /// <summary>
    /// This partial adds extended Match properties
    /// </summary>
    public class MatchSet
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }

        public         int   MatchId        { get; set; }
        [ForeignKey("MatchId")]
        public virtual Match Match          { get; set; }
        public         int   PlayerOneScore { get; set; }
        public         int   PlayerTwoScore { get; set; }
    }
}