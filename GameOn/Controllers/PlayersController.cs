using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using GameOn.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOn.Web.Controllers
{
    /// <summary>
    /// A controller for Player related actions
    /// </summary>
    public class PlayersController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        /// <summary>
        /// Returns a list of Players in order of their place on the ladder
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Ladder()
        {
            ViewData.Model = await _playerService.GetListOfPlayersWithMatchSummaries();
            return View("Ladder_m");
        }

        /// <summary>
        /// Gets Player's details and match summaries as CSV
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPlayersCsv()
        {
            var playersCsv = await _playerService.GetListOfPlayersWithMatchSummaries();

            await using MemoryStream stream = new MemoryStream();
            await using StreamWriter writeFile = new StreamWriter(stream);
            using CsvWriter csv = new CsvWriter(writeFile);

            csv.Configuration.RegisterClassMap<PlayerMatchSummaryMapCvs>();
            csv.WriteRecords(playersCsv);
            stream.Position = 0; //reset stream

            return File(stream, "application/octet-stream", "players.csv");
        }

        /// <summary>
        /// Gets Sparklines data for optionally one or all players
        /// </summary>
        /// <returns>Sparklines data as JSON</returns>
        [HttpGet]
        public IActionResult GetSparklinesData(int? playerId)
        {
            IList<Tuple<int, string>> data;
            if (playerId.HasValue)
            {
                data = new List<Tuple<int, string>> {_playerService.GetRankHistoryDataForSparkLine(playerId.Value)};
            }
            else
            {
                data = _playerService.GetRankHistoryDataForSparkLine();
            }

            return new JsonResult(data);
        }
    }
}
