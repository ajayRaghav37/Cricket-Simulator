using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.TableEntities;
using static Logging.Logger;

namespace Repositories
{
    public static class SquadsRepository
    {
        public async static Task<List<Squads>> GetAll(IEnumerable<int> teamIds)
        {
            LogEntry(inputParams: teamIds);

            List<Squads> squads = await MobileServiceConnection.MobileService.GetTable<Squads>().Where(item => teamIds.Contains(item.TeamId)).ToListAsync();

            LogExit(returnValue: squads);
            return squads;
        }

        public async static Task InsertSingle(Squads squad)
        {
            LogEntry(inputParams: squad);

            await MobileServiceConnection.MobileService.GetTable<Squads>().InsertAsync(squad);

            LogExit();
        }
    }
}
