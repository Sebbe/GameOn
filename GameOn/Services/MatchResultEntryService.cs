using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using GameOn.Web.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GameOn.Web.Services
{
    /// <summary>
    ///   A service for quickly adding match and game results
    /// </summary>
    public class MatchResultEntryService : IMatchResultEntryService
    {
        private const string PlayerVPlayerNameFormat = "{0} v {1}";
        private readonly IRatingHelper _ratingHelper;
        private readonly IRankHistoryLoggingService _rankHistoryLoggingService;
        private readonly ITeamService _teamService;
        private readonly ITimelineService _timelineService;
        private readonly GameOnContext _gameOnContext;

        public MatchResultEntryService(IRatingHelper ratingHelper, IRankHistoryLoggingService rankHistoryLoggingService, ITeamService teamService, ITimelineService timelineService, GameOnContext gameOnContext)
        {
            _ratingHelper = ratingHelper;
            _rankHistoryLoggingService = rankHistoryLoggingService;
            _teamService = teamService;
            _timelineService = timelineService;
            _gameOnContext = gameOnContext;
        }


        //public MatchResultEntryService(GameOnEntities entities)
        //{
        //    Entities = entities;
        //    RatingHelper = new DurrantEloRatingHelper();
        //    RankHistoryLoggingService = new RankHistoryLoggingService(entities);
        //    PlayerService = new PlayerService(entities);
        //    TimelineService = new TimelineService();
        //}

        /// <summary>
        ///   Adds a Match, returning the match ID
        /// </summary>
        /// <param name = "playedDate">The date the match was played</param>
        /// <param name = "team1Id">Player 1's ID</param>
        /// <param name = "team2Id">Player 2's ID</param>
        /// <param name="sets">Sets played</param>
        /// <returns>The new Match ID</returns>
        private async Task<Match> AddMatch(DateTime playedDate, int team1Id, int team2Id, IEnumerable<MatchSet> sets)
        {
            var finishedSets = sets.Where(x => x.PlayerOneScore != x.PlayerTwoScore).ToList();

            var match = new Match
                            {
                                Date = playedDate,
                                TeamOne = _gameOnContext.Teams.FirstOrDefault(p => p.Id == team1Id),
                                TeamTwo = _gameOnContext.Teams.FirstOrDefault(p => p.Id == team2Id),
                                Finished = true,
                                Sets = finishedSets
            };

            if (match.TeamOne == null) throw new ArgumentException("Teams 1 ID is not valid", "team1Id");
            if (match.TeamTwo == null) throw new ArgumentException("Teams 2 ID is not valid", "team2Id");
            if (match.TeamOne == match.TeamTwo) throw new ArgumentException("Teams 1 and 2 may not be the same");

            match.Name = string.Format(PlayerVPlayerNameFormat, match.TeamOne.Name, match.TeamTwo.Name);

            // TODO Check match Type and check if the match is finished, or is that up to controller or maybe the frontend. No idea?
            var totalSetPlayerOneWins = finishedSets.Count(x => x.PlayerOneScore > x.PlayerTwoScore);
            var totalSetPlayerTwoWins = finishedSets.Count(x => x.PlayerTwoScore > x.PlayerOneScore);
            match.WinnerTeamId = totalSetPlayerOneWins > totalSetPlayerTwoWins ? team1Id : team2Id;
            match.LoserTeamId = match.WinnerTeamId == team1Id ? team2Id : team1Id;

            string resultString = string.Empty;

            foreach (MatchSet matchSet in finishedSets)
            {
                if (match.WinnerTeamId == team1Id)
                {
                    resultString = $"{resultString}{matchSet.PlayerOneScore}-{matchSet.PlayerTwoScore},";
                }
                else
                {
                    resultString = $"{resultString}{matchSet.PlayerTwoScore}-{matchSet.PlayerOneScore},";
                }
            }

            resultString = resultString.Remove(resultString.Length - 1);

            match.FinishScore = resultString;

            await _gameOnContext.Matches.AddAsync(match);
            await _gameOnContext.SaveChangesAsync();
            return match;
        }

        /// <summary>
        /// Adds a new Match, marked as played, and updates the rankings of each player based on who won, and their current rankings
        /// </summary>
        /// <param name="team1">Team 1's ID</param>
        /// <param name="team2">Team 2's ID</param>
        /// <param name="matchSets">The sets played</param>
        /// <returns>A Match entity expressing the results of the match</returns>
        public async Task<Match> Played(int team1, int team2, IList<MatchSet> matchSets)
        {
            var match = await AddMatch(DateTime.Now, team1, team2, matchSets);
            //match = _gameOnContext.Matches
            //                      .Include(x => x.WinnerPlayer)
            //                      .Include(x => x.LoserPlayer)
            //                      .Include(x => x.PlayerOne)
            //                      .Include(x => x.PlayerTwo)
            //                      .First(x => x.Id == match.Id);

            // Get current leader
            var leader = await _teamService.GetTopRankingTeam(match.TeamOne.IsDouble);

            // Calculate new rankings
            _ratingHelper.UpdateMatchPlayersRanks(match);
            await _gameOnContext.SaveChangesAsync();

            await SaveRankHistory(match.TeamOne);
            await SaveRankHistory(match.TeamTwo);

            // timeline the match
            _timelineService.AddMessage($"{match.WinnerTeam.Name} beat {match.LoserTeam.Name} ({match.FinishScore})");

            // check if leader has changed, if so add to timeline
            var newLeader = await _teamService.GetTopRankingTeam(match.TeamOne.IsDouble);

            if (leader != newLeader)
            {
                _timelineService.AddMessage($"{match.WinnerTeam.Name} has replaced {leader.Name} as the new top ranking team with a rating of {match.WinnerTeam.CurrentRank}!");
            }

            return match;
        }

        private async Task SaveRankHistory(Team team)
        {
            await _rankHistoryLoggingService.SaveRankHistory(team);
        }
    }
}