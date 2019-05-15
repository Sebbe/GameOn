using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    public class TournamentMatchSchedule
    {
        [Key]
        public int Id { get; set; }

        public         int        Round        { get; set; }
        public         DateTime   Date         { get; set; }

        public         int        TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament   { get; set; }

        public         int?    PlayerOneId { get; set; }

        [ForeignKey("PlayerOneId")]
        public virtual Player PlayerOne   { get; set; }

        public         int?    PlayerTwoId { get; set; }

        [ForeignKey("PlayerTwoId")]
        public virtual Player PlayerTwo   { get; set; }

        public int? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual TournamentGroup Group { get; set; }

        public         int?  MatchId { get; set; }

        [ForeignKey("MatchId")]
        public virtual Match Match   { get; set; }
    }
}