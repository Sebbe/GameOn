using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name        { get; set; }
        public int    CurrentRank { get; set; }

        public bool IsDouble { get; set; }

        public int    PlayerOneId { get; set; }
        public Player PlayerOne   { get; set; }

        public int?   PlayerTwoId { get; set; }
        public Player PlayerTwo   { get; set; }

        [InverseProperty("TeamOne")]
        public ICollection<Match> MatchesAsTeamOne { get; set; }

        [InverseProperty("TeamTwo")]
        public ICollection<Match> MatchesAsTeamTwo { get; set; }

        [InverseProperty("Team")]
        public ICollection<RankHistory> RankHistory { get; set; }

        [InverseProperty("Team")]
        public ICollection<TournamentTeam> Tournaments { get; set; }

        //[InverseProperty("Player")]
        public ICollection<TournamentGroupTeam> TournamentGroups { get; set; }
    }
}