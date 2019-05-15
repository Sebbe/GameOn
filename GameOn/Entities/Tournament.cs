using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameOn.Web.Entities
{
    public enum TournamentType
    {
        Elimination = 0,
        GroupsThenElimination = 1,
        AllPlayAgainstEachOther = 2
    }

    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        public string Name          { get; set; }
        public int    MaxPlayers    { get; set; }
        public int    MinRankPoints { get; set; }

        [InverseProperty("Tournament")]
        public ICollection<TournamentMatchSchedule> Schedules { get; set; }

        //[InverseProperty("Tournament")]
        public ICollection<TournamentPlayer> Players   { get; set; }

        [InverseProperty("Tournament")]
        public ICollection<Match> Matches { get; set; }

        [InverseProperty("Tournament")]
        public ICollection<TournamentGroup> Groups { get; set; }
    }
}