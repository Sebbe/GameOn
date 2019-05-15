using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    /// <summary>
    /// This partial adds extended functionality to the Player entity
    /// </summary>
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public string Name        { get; set; }
        public string FullName    { get; set; }
        public string Email       { get; set; }
        public int    CurrentRank { get; set; }

        [InverseProperty("PlayerOne")]
        public ICollection<Match> MatchesAsPlayerOne { get; set; }

        [InverseProperty("PlayerTwo")]
        public ICollection<Match> MatchesAsPlayerTwo { get; set; }

        [InverseProperty("Player")]
        public ICollection<RankHistory> RankHistory { get; set; }

        [InverseProperty("Player")]
        public ICollection<TournamentPlayer> Tournaments { get; set; }

        //[InverseProperty("Player")]
        public ICollection<TournamentGroupPlayer> TournamentGroups { get; set; }

        /// <summary>
        /// Returns the Player's name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }
    }
}