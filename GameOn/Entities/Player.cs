using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GameOn.Web.Entities
{
    /// <summary>
    /// This partial adds extended functionality to the Player entity
    /// </summary>
    public class Player : IdentityUser<int>
    {
        public string Name        { get; set; }
        public string FullName    { get; set; }
        
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