using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    public interface IPlayerRegistrationService
    {
        /// <summary>
        /// Adds a Player, returning the new ID
        /// </summary>
        /// <param name="player">A Player DTO</param>
        Task<int> AddPlayer(Player player);
    }
}