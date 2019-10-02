using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameOn.Web.Entities
{
    public class TournamentTeam
    {
        public         int        TournamentId { get; set; }
        public virtual Tournament Tournament   { get; set; }
        public         int        TeamId     { get; set; }
        public virtual Team     Team       { get; set; }
    }
}