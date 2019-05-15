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
        private readonly IPlayerService _playerService;
        private readonly ITimelineService _timelineService;
        private readonly GameOnContext _gameOnContext;

        public MatchResultEntryService(IRatingHelper ratingHelper, IRankHistoryLoggingService rankHistoryLoggingService, IPlayerService playerService, ITimelineService timelineService, GameOnContext gameOnContext)
        {
            _ratingHelper = ratingHelper;
            _rankHistoryLoggingService = rankHistoryLoggingService;
            _playerService = playerService;
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
        /// <param name = "player1Id">Player 1's ID</param>
        /// <param name = "player2Id">Player 2's ID</param>
        /// <param name="winnerId">The winning player's ID</param>
        /// <param name="score">The score of the match, expressed as a string. e.g. 10-3, 3-10, 10-12</param>
        /// <returns>The new Match ID</returns>
        private async Task<Match> AddMatch(DateTime playedDate, int player1Id, int player2Id, IList<MatchSet> sets)
        {
            var finishedSets = sets.Where(x => x.PlayerOneScore != x.PlayerTwoScore).ToList();

            var match = new Match
                            {
                                Date = playedDate,
                                PlayerOne = _gameOnContext.Players.FirstOrDefault(p => p.Id == player1Id),
                                PlayerTwo = _gameOnContext.Players.FirstOrDefault(p => p.Id == player2Id),
                                Finished = true,
                                Sets = finishedSets
            };

            if (match.PlayerOne == null) throw new ArgumentException("Player 1 ID is not valid", "player1Id");
            if (match.PlayerTwo == null) throw new ArgumentException("Player 2 ID is not valid", "player2Id");
            if (match.PlayerOne == match.PlayerTwo) throw new ArgumentException("Players 1 and 2 may not be the same");

            match.Name = string.Format(PlayerVPlayerNameFormat, match.PlayerOne.Name, match.PlayerTwo.Name);

            // TODO Check match Type and check if the match is finished, or is that up to controller or maybe the frontend. No idea?
            var totalSetPlayerOneWins = finishedSets.Count(x => x.PlayerOneScore > x.PlayerTwoScore);
            var totalSetPlayerTwoWins = finishedSets.Count(x => x.PlayerTwoScore > x.PlayerOneScore);
            match.WinnerPlayerId = totalSetPlayerOneWins > totalSetPlayerTwoWins ? player1Id : player2Id;
            match.LoserPlayerId = match.WinnerPlayerId == player1Id ? player2Id : player1Id;

            string resultString = string.Empty;

            foreach (MatchSet matchSet in finishedSets)
            {
                if (match.WinnerPlayerId == player1Id)
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
        /// <param name="player1">Player 1's ID</param>
        /// <param name="player2">Player 2's ID</param>
        /// <param name="matchSets">The sets played</param>
        /// <returns>A Match entity expressing the results of the match</returns>
        public async Task<Match> Played(int player1, int player2, IList<MatchSet> matchSets)
        {
            var match = await AddMatch(DateTime.Now, player1, player2, matchSets);
            //match = _gameOnContext.Matches
            //                      .Include(x => x.WinnerPlayer)
            //                      .Include(x => x.LoserPlayer)
            //                      .Include(x => x.PlayerOne)
            //                      .Include(x => x.PlayerTwo)
            //                      .First(x => x.Id == match.Id);

            // Get current leader
            var leader = await _playerService.GetTopRankingPlayer();

            // Calculate new rankings
            _ratingHelper.UpdateMatchPlayersRanks(match);
            await _gameOnContext.SaveChangesAsync();

            await SaveRankHistory(match.PlayerOne);
            await SaveRankHistory(match.PlayerTwo);

            // timeline the match
            _timelineService.AddMessage($"{match.WinnerPlayer.Name} beat {match.LoserPlayer.Name} ({match.FinishScore})");

            // check if leader has changed, if so add to timeline
            var newLeader = await _playerService.GetTopRankingPlayer();

            if (leader != newLeader)
            {
                _timelineService.AddMessage($"{match.WinnerPlayer.Name} has replaced {leader.Name} as the new top ranking player with a rating of {match.WinnerPlayer.CurrentRank}!");
            }

            return match;
        }

        private async Task SaveRankHistory(Player player)
        {
            await _rankHistoryLoggingService.SaveRankHistory(player);
        }
    }
}