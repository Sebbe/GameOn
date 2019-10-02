using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using GameOn.Web.Models;

namespace GameOn.Web.Services
{
    public interface ITeamService
    {
        /// <summary>
        /// Gets a single team by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Team> GetTeam(int id);

        /// <summary>
        /// Returns a ranked list of players, sorted by their ELO rating.
        /// </summary>
        /// <returns></returns>
        Task<IList<Team>> GetTeamsByRank(bool isDouble = false);

        /// <summary>
        /// Gets the top ranking team, mon
        /// </summary>
        Task<Team> GetTopRankingTeam(bool isDouble = false);

        /// <summary>
        /// Gets a summary of all of the matches that a Player has played
        /// </summary>
        Task<TeamMatchesSummary> GetTeamMatchesSummary(int teamId);

        /// <summary>
        /// Gets a list of players and summaries of all of the matches each Player has played
        /// </summary>
        Task<IList<TeamMatchesSummary>> GetListOfTeamsWithMatchSummaries(bool isDouble = false);

        /// <summary>
        /// Gets a list of player IDs and comma-separated Rank data for drawing players' sparklines
        /// </summary>
        /// <returns>A list of tuples, each a player ID as int, and a list of Ranks as a comma-separated string</returns>
        IList<Tuple<int, string>> GetRankHistoryDataForSparkLine(bool isDouble = false);

        /// <summary>
        /// Gets a player ID and comma-separated Rank data for drawing a player's sparkline
        /// </summary>
        /// <param name="teamId">A team ID. Returns a </param>
        /// <returns>A tuple: a player ID as int, and a list of Ranks as a comma-separated string</returns>
        Tuple<int, string> GetRankHistoryDataForSparkLine(int teamId);

    }
}