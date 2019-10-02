using System.ComponentModel.DataAnnotations;

namespace GameOn.Web.Entities
{
    public class AdminConfiguration
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
