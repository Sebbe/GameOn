using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameOn.Web.Entities
{
    public class TournamentGroupTeam
    {
        public         int             TournamentGroupId { get; set; }
        public virtual TournamentGroup TournamentGroup   { get; set; }
        public         int             TeamId            { get; set; }
        public virtual Team            Team              { get; set; }
    }
}