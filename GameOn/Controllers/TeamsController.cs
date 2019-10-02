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
        private readonly ITeamService _teamService;

        public PlayersController(ITeamService playerService)
        {
            _teamService = playerService;
        }


        /// <summary>
        /// Returns a list of Players in order of their place on the ladder
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Ladder(bool isDouble = false)
        {
            ViewData.Model = await _teamService.GetListOfTeamsWithMatchSummaries(isDouble);
            return View("Ladder_m");
        }

        /// <summary>
        /// Gets Player's details and match summaries as CSV
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPlayersCsv(bool isDouble = false)
        {
            var playersCsv = await _teamService.GetListOfTeamsWithMatchSummaries(isDouble);

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
        public IActionResult GetSparklinesData(int? teamId)
        {
            IList<Tuple<int, string>> data;
            if (teamId.HasValue)
            {
                data = new List<Tuple<int, string>> {_teamService.GetRankHistoryDataForSparkLine(teamId.Value)};
            }
            else
            {
                data = _teamService.GetRankHistoryDataForSparkLine();
            }

            return new JsonResult(data);
        }
    }
}
