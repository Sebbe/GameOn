using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    public enum MatchType
    {
        BestOfOne   = 0,
        BestOfThree = 1,
        BestOfFive  = 2
    }

    public class Match
    {
        [Key]
        public int Id { get; set; }

        public MatchType MatchType { get; set; }

        public bool      Finished  { get; set; }
        public DateTime  Date      { get; set; } = DateTime.UtcNow;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime   { get; set; }
        public string    Name      { get; set; }
        public string FinishScore { get; set; }

        public int? PlayerOneId { get; set; }

        [ForeignKey("PlayerOneId")]
        public virtual Player PlayerOne { get; set; }

        public int? PlayerTwoId { get; set; }

        [ForeignKey("PlayerTwoId")]
        public virtual Player PlayerTwo { get; set; }

        [InverseProperty("Match")]
        public ICollection<MatchSet> Sets { get; set; }

        public int? WinnerPlayerId { get; set; }

        [ForeignKey("WinnerPlayerId")]
        public virtual Player WinnerPlayer { get; set; }

        public int? LoserPlayerId { get; set; }

        [ForeignKey("LoserPlayerId")]
        public virtual Player LoserPlayer { get; set; }

        public int? TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }

        public int? TournamentGroupId { get; set; }

        [ForeignKey("TournamentGroupId")]
        public virtual TournamentGroup TournamentGroup { get; set; }
    }
}