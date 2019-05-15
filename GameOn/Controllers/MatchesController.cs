using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using GameOn.Web.Models;
using GameOn.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOn.Web.Controllers
{
    /// <summary>
    /// A Matches controller. Controls views for listing and playing matches
    /// </summary>
    public class MatchesController : Controller
    {
        private readonly GameOnContext _gameOnContext;
        private readonly IMatchResultEntryService _matchResultEntryService;
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;
        private readonly IPlayerRegistrationService _playerRegistrationService;

        public MatchesController(GameOnContext gameOnContext, IMatchResultEntryService matchResultEntryService, IPlayerService playerService, IMatchService matchService, IPlayerRegistrationService playerRegistrationService)
        {
            _gameOnContext = gameOnContext;
            _matchResultEntryService = matchResultEntryService;
            _playerService = playerService;
            _matchService = matchService;
            _playerRegistrationService = playerRegistrationService;
        }

        /// <summary>
        /// Gets a list of the last 100 matches played, optionally for a given player's ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int? playerId)
        {
            var model = new PlayerMatchListModel();

            if (playerId.HasValue)
            {
                model.Matches = await _matchService.GetMatches(playerId.Value, new { PageSize = 100 });
                model.Player = await _playerService.GetPlayer(playerId.Value);
                model.PlayerMatchesSummary = await _playerService.GetPlayerMatchesSummary(playerId.Value);
                ViewData.Model = model;
                return View("Index_m");                
            }

            model.Matches = await _matchService.GetMatches(new { PageSize = 100 });
            ViewData.Model = model;
            return View("Index_m");
        }

        /// <summary>
        /// Returns a view that prompts to Start a new Game. When no player1 ID is provided, prompts for Player 1. When a player1 ID is provided, prompts for player 2
        /// </summary>
        /// <param name="player1"></param>
        /// <returns></returns>
        [HttpGet]
        //[ResponseCache(Duration = 0, NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> New(int? player1)
        {
            ViewBag.Player1 = player1;
            ViewData.Model = await _playerService.GetPlayersByRank();
            return View("New_m");
        }

        /// <summary>
        /// Starts a new game and creates a player at the same time. If player1 is not provided, the playerName will be used to create a new player, who will be player 1 for this match, and
        /// the action will redirect to prompt for player 2. If player1 is provided, playerName will be used to create a new player who will be player 2 for this match, and the action will
        /// redirect to the Play action.
        /// </summary>
        /// <param name="player1">Optional. Player 1's ID</param>
        /// <param name="playerName">Required. A Player name.</param>
        /// <returns>Returns the result of either the New or Play action.</returns>
        [HttpPost]
        public async Task<IActionResult> New(int? player1, string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentNullException("playerName");

            int playerId = await _playerRegistrationService.AddPlayer(new Player {Name = playerName, FullName = playerName, Email = playerName, CurrentRank = 1000});

            if (player1.HasValue) return await Play(player1.Value, playerId);
            return await New(playerId);
        }

        /// <summary>
        /// Returns a view to display while a game is being played.
        /// </summary>
        [HttpGet]
        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new []{"player1","player2"})]
        public async Task<IActionResult> Play(int player1, int player2)
        {
            var model = new PlayModel();
            model.Player1 = await _playerService.GetPlayer(player1);
            model.Player2 = await _playerService.GetPlayer(player2);
            ViewData.Model = model;
            return View("Play_m");
        }

        /// <summary>
        /// Redirects to the New action. Here to prevent missing param exceptions in jQuery Mobile
        /// </summary>
        [HttpGet]
        public IActionResult Played()
        {
            return RedirectToAction("New");
        }

        /// <summary>
        /// Enters the results of a Match that has just been played.
        /// </summary>
        /// <param name="player1">Player 1's ID</param>
        /// <param name="player2">Player 2's ID</param>
        /// <param name="winnerId">The winning player's ID</param>
        /// <param name="score">The score expressed as a string</param>
        /// <returns>The Played view</returns>
        [HttpPost]
        public async Task<IActionResult> Played(int player1, int player2, IList<MatchSet> sets)
        {
            ViewData.Model = await _matchResultEntryService.Played(player1, player2, sets);
            return View("Played_m");
        }
    }
}
