using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOn.Web.Entities
{
    public class RankHistory
    {
        [Key]
        public int Id { get; set; }

        public         DateTime Date     { get; set; }
        public         int      Rank     { get; set; }
        public         int      PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual Player   Player   { get; set; }
    }
}