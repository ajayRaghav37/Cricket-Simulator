using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Logging.Logger;
using Repositories.TableEntities;

namespace Repositories
{
    public static class PlayerRepository
    {
        public async static void GetSingle(Players player)
        {
            LogEntry(inputParams: player);

            List<Players> players = await MobileServiceConnection.MobileService.GetTable<Players>().Where(item => item.PlayerId == player.PlayerId).ToListAsync();
            player = players.First();

            LogExit();
        }

        public async static Task<List<Players>> GetAll(IEnumerable<int> playerIds)
        {
            LogEntry(inputParams: playerIds);

            List<Players> players = await MobileServiceConnection.MobileService.GetTable<Players>().Where(item => playerIds.Contains(item.PlayerId)).ToListAsync();

            LogExit(returnValue: players);

            return players;
        }

        public async static Task InsertSingle(Players player)
        {
            LogEntry(inputParams: player);

            await MobileServiceConnection.MobileService.GetTable<Players>().InsertAsync(player);

            LogExit();
        }
    }
}
