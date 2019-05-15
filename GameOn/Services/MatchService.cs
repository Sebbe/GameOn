using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOn.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameOn.Web.Services
{
    public class MatchService : IMatchService
    {
        private readonly GameOnContext _gameOnContext;

        public MatchService(GameOnContext gameOnContext)
        {
            _gameOnContext = gameOnContext;
        }


        /// <summary>
        /// Returns a page of pool matches
        /// </summary>
        /// <param name="args">A dynamic object of Arguments. PageSize = The size of the page to return. If missing, will return all items</param>
        public async Task<IList<Match>> GetMatches(dynamic args)
        {
            var query = BasicMatchQuery();
            if (args.PageSize != null)
            {
                query = query
                    .Take((int) args.PageSize);
            }

            return await query.OrderByDescending(m => m.Id).ToListAsync();
        }

        /// <summary>
        /// Returns a page of pool matches for a player
        /// </summary>
        /// <param name="playerId">The player ID</param>
        /// <param name="args">A dynamic object of Arguments. PageSize = The size of the page to return. If missing, will return all items</param>
        public async Task<IList<Match>> GetMatches(int playerId, dynamic args)
        {
            var query = BasicMatchQuery().Where(m => m.Finished && (m.PlayerOneId == playerId || m.PlayerTwoId == playerId));

            if (args.PageSize != null)
            {
                query = query.OrderByDescending(m => m.Id).Take((int) args.PageSize);
            }

            return await query.ToListAsync();
        }

        private IQueryable<Match> BasicMatchQuery()
        {
            return _gameOnContext.Matches
                                 .Include(x => x.WinnerPlayer)
                                 .Include(x => x.LoserPlayer)
                                 .Include(x => x.PlayerOne)
                                 .Include(x => x.PlayerTwo)
                                 .Where(m => m.Finished);
        }
    }
}