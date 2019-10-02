using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using GameOn.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace GameOn.Web
{
    /// <summary>
    /// An implementation of <see cref="IView"/> for rendering <see cref="PlayerMatchesSummary"/> data as CSV
    /// </summary>
    public class PlayerMatchSummaryMapCvs : ClassMap<TeamMatchesSummary>
    {
        public PlayerMatchSummaryMapCvs()
        {
            Map(m => m.Team.Name).Index(0);
            Map(m => m.Played).Index(1);
            Map(m => m.Won).Index(2);
            Map(m => m.Lost).Index(3);
            Map(m => m.Team.CurrentRank).Index(4);
            Map(m => m.HighestRanking).Index(5);
            Map(m => m.LowestRanking).Index(6);
        }
    }
}