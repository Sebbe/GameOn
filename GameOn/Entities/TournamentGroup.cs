using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    public class TournamentGroup
    {
        [Key]
        public int Id { get; set; }

        public string Name         { get; set; }
        public int    TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }

        //[InverseProperty("TournamentGroup")]
        public ICollection<TournamentGroupPlayer> Players { get; set; }

        [InverseProperty("TournamentGroup")]
        public ICollection<Match> Matches { get; set; }
    }
}