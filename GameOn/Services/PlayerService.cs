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
    public class TeamService : ITeamService
    {
        private readonly GameOnContext _gameOnContext;

        public TeamService(GameOnContext gameOnContext)
        {
            _gameOnContext = gameOnContext;
        }

        /// <summary>
        /// Gets a single player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Team> GetTeam(int id)
        {
            return _gameOnContext.Teams.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Returns a ranked list of players, sorted by their ELO rating.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Team>> GetTeamsByRank(bool isDouble = false)
        {
            return await _gameOnContext.Teams.Where(x => x.IsDouble == isDouble).OrderByDescending(p => p.CurrentRank).ToListAsync();
        }

        public Task<Team> GetTopRankingTeam(bool isDouble = false)
        {
            return _gameOnContext.Teams.Where(x => x.IsDouble == isDouble).OrderByDescending(p => p.CurrentRank).FirstOrDefaultAsync();
        }

        private IQueryable<TeamMatchesSummary> GetTeamMatchesSummaryQuery(bool isDouble = false)
        {
            var query = from p in _gameOnContext.Teams
                        where p.IsDouble == isDouble
                        select
                            new TeamMatchesSummary
                            {
                                Team = p,
                                HighestRanking = p.RankHistory.Max(rh => (int?)rh.Rank),
                                LowestRanking = p.RankHistory.Min(rh => (int?)rh.Rank),
                                Won = p.MatchesAsTeamOne.Count(y => y.WinnerTeamId == p.Id) + p.MatchesAsTeamTwo.Count(y => y.WinnerTeamId == p.Id),
                                Played = p.MatchesAsTeamOne.Count + p.MatchesAsTeamTwo.Count
                            };
            return query;
        }

        /// <summary>
        /// Gets a summary of all of the matches that a team has played
        /// </summary>
        public async Task<TeamMatchesSummary> GetTeamMatchesSummary(int teamId)
        {
            var query = GetTeamMatchesSummaryQuery();
            return await query.FirstOrDefaultAsync(p => p.Team.Id == teamId);
        }

        /// <summary>
        /// Gets a list of players and summaries of all of the matches each Player has played, sorted by rank
        /// </summary>
        public async Task<IList<TeamMatchesSummary>> GetListOfTeamsWithMatchSummaries(bool isDouble = false)
        {
            var query = GetTeamMatchesSummaryQuery(isDouble);
            return await query.OrderByDescending(p=>p.Team.CurrentRank).ToListAsync();
        }

        /// <summary>
        /// Gets a list of player IDs and comma-separated Rank data for drawing players' sparklines
        /// </summary>
        /// <returns>A list of tuples, each a team ID as int, and a list of Ranks as a comma-separated string</returns>
        public IList<Tuple<int, string>> GetRankHistoryDataForSparkLine(bool isDouble = false)
        {
            var query = from h in _gameOnContext.RankHistory 
                        orderby h.TeamId, h.Id
                        select new {h.TeamId, h.Rank};

            var results = new List<Tuple<int, string>>();

            int lastPlayerId = 0;
            string data = "";
            bool first = true;
            foreach (var history in query)
            {
                if (lastPlayerId != 0 && lastPlayerId != history.TeamId)
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
                lastPlayerId = history.TeamId;
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
