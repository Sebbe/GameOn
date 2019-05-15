using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using GameOn.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOn.Web.Services
{
    /// <summary>
    /// A read-only service for retrieving players
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly GameOnContext _gameOnContext;

        public PlayerService(GameOnContext gameOnContext)
        {
            _gameOnContext = gameOnContext;
        }

        /// <summary>
        /// Gets a single player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Player> GetPlayer(int id)
        {
            return _gameOnContext.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Returns a ranked list of players, sorted by their ELO rating.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Player>> GetPlayersByRank()
        {
            return await _gameOnContext.Players.OrderByDescending(p => p.CurrentRank).ToListAsync();
        }

        public Task<Player> GetTopRankingPlayer()
        {
            return _gameOnContext.Players.OrderByDescending(p => p.CurrentRank).FirstOrDefaultAsync();
        }

        private IQueryable<PlayerMatchesSummary> GetPlayerMatchesSummaryQuery()
        {
            var query = from p in _gameOnContext.Players
                        select
                            new PlayerMatchesSummary
                            {
                                Player = p,
                                HighestRanking = p.RankHistory.Max(rh => (int?)rh.Rank),
                                LowestRanking = p.RankHistory.Min(rh => (int?)rh.Rank),
                                Won = p.MatchesAsPlayerOne.Count(y => y.WinnerPlayerId == p.Id) + p.MatchesAsPlayerTwo.Count(y => y.WinnerPlayerId == p.Id),
                                Played = p.MatchesAsPlayerOne.Count + p.MatchesAsPlayerTwo.Count
                            };
            return query;
        }

        /// <summary>
        /// Gets a summary of all of the matches that a Player has played
        /// </summary>
        public async Task<PlayerMatchesSummary> GetPlayerMatchesSummary(int playerId)
        {
            var query = GetPlayerMatchesSummaryQuery();
            return await query.FirstOrDefaultAsync(p => p.Player.Id == playerId);
        }

        /// <summary>
        /// Gets a list of players and summaries of all of the matches each Player has played, sorted by rank
        /// </summary>
        public async Task<IList<PlayerMatchesSummary>> GetListOfPlayersWithMatchSummaries()
        {
            var query = GetPlayerMatchesSummaryQuery();
            return await query.OrderByDescending(p=>p.Player.CurrentRank).ToListAsync();
        }

        /// <summary>
        /// Gets a list of player IDs and comma-separated Rank data for drawing players' sparklines
        /// </summary>
        /// <returns>A list of tuples, each a player ID as int, and a list of Ranks as a comma-separated string</returns>
        public IList<Tuple<int, string>> GetRankHistoryDataForSparkLine()
        {
            var query = from h in _gameOnContext.RankHistory 
                        orderby h.PlayerId, h.Id
                        select new {h.PlayerId, h.Rank};

            var results = new List<Tuple<int, string>>();

            int lastPlayerId = 0;
            string data = "";
            bool first = true;
            foreach (var history in query)
            {
                if (lastPlayerId != 0 && lastPlayerId != history.PlayerId)
                {
                    var result = new Tuple<int, string>(lastPlayerId, data);
                    results.Add(result);
                    data = "";
                    first = true;
                }

                if (!first)
                {
                    data += ",";
                }
                
                data += history.Rank;
                lastPlayerId = history.PlayerId;
                first = false;
            }

            if (lastPlayerId != 0)
            {
                var result = new Tuple<int, string>(lastPlayerId, data);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Gets a player ID and comma-separated Rank data for drawing a player's sparkline
        /// </summary>
        /// <param name="playerId">A player ID. Returns a </param>
        /// <returns>A tuple: a player ID as int, and a list of Ranks as a comma-separated string</returns>
        public Tuple<int, string> GetRankHistoryDataForSparkLine(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}
