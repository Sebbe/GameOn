using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using GameOn.Web.Models;

namespace GameOn.Web.Services
{
    public interface IPlayerService
    {
        /// <summary>
        /// Gets a single player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Player> GetPlayer(int id);

        /// <summary>
        /// Returns a ranked list of players, sorted by their ELO rating.
        /// </summary>
        /// <returns></returns>
        Task<IList<Player>> GetPlayersByRank();

        /// <summary>
        /// Gets the top ranking player, mon
        /// </summary>
        Task<Player> GetTopRankingPlayer();

        /// <summary>
        /// Gets a summary of all of the matches that a Player has played
        /// </summary>
        Task<PlayerMatchesSummary> GetPlayerMatchesSummary(int playerId);

        /// <summary>
        /// Gets a list of players and summaries of all of the matches each Player has played
        /// </summary>
        Task<IList<PlayerMatchesSummary>> GetListOfPlayersWithMatchSummaries();

        /// <summary>
        /// Gets a list of player IDs and comma-separated Rank data for drawing players' sparklines
        /// </summary>
        /// <returns>A list of tuples, each a player ID as int, and a list of Ranks as a comma-separated string</returns>
        IList<Tuple<int, string>> GetRankHistoryDataForSparkLine();

        /// <summary>
        /// Gets a player ID and comma-separated Rank data for drawing a player's sparkline
        /// </summary>
        /// <param name="playerId">A player ID. Returns a </param>
        /// <returns>A tuple: a player ID as int, and a list of Ranks as a comma-separated string</returns>
        Tuple<int, string> GetRankHistoryDataForSparkLine(int playerId);

    }
}