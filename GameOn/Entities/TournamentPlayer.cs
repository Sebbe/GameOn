using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameOn.Web.Entities
{
    public class TournamentPlayer
    {
        public         int        TournamentId { get; set; }
        public virtual Tournament Tournament   { get; set; }
        public         int        PlayerId     { get; set; }
        public virtual Player     Player       { get; set; }
    }
}