using System;
using System.Data;
using System.Threading.Tasks;
using GameOn.Web.Entities;

namespace GameOn.Web.Services
{
    /// <summary>
    /// A service for registering and editing players
    /// </summary>
    public class PlayerRegistrationService : IPlayerRegistrationService
    {
        private readonly GameOnContext _gameOnContext;

        public PlayerRegistrationService(GameOnContext gameOnContext)
        {
            _gameOnContext = gameOnContext;
        }

        /// <summary>
        /// Adds a Player, returning the new ID
        /// </summary>
        /// <param name="player">A Player DTO</param>
        public async Task<int> AddPlayer(Player player)
        {
            if (player.Id != 0) throw new InvalidOperationException("Don't use the AddPlayer method to update a player. Use EditPlayer instead");
            await _gameOnContext.Players.AddAsync(player);
            var newTeam = new Team();
            newTeam.CurrentRank = 1000;
            newTeam.IsDouble = false;
            newTeam.Name = player.Name;

            var rankHistory = new RankHistory {Team = newTeam, Rank = newTeam.CurrentRank, Date = DateTime.Now};
            await _gameOnContext.RankHistory.AddAsync(rankHistory);

            await _gameOnContext.SaveChangesAsync();
            return player.Id;
        }
    }
}
