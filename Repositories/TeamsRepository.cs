using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.TableEntities;
using static Logging.Logger;

namespace Repositories
{
    public static class TeamsRepository
    {
        public async static void GetSingle(Teams team)
        {
            LogEntry(inputParams: team);

            List<Teams> teams = await MobileServiceConnection.MobileService.GetTable<Teams>().Where(item => item.TeamId == team.TeamId).ToListAsync();
            team = teams.First();

            LogExit();
        }

        public async static void GetMultiple(List<Teams> teams)
        {
            LogEntry(inputParams: teams);

            List<int> teamIds = teams.Select(item => item.TeamId).ToList();
            teams = await MobileServiceConnection.MobileService.GetTable<Teams>().Where(item => teamIds.Contains(item.TeamId)).ToListAsync();

            LogExit();
        }

        public async static Task<List<Teams>> GetAll()
        {
            LogEntry();

            List<Teams> teams = await MobileServiceConnection.MobileService.GetTable<Teams>().Where(item => item.TeamId > 0).ToListAsync();

            LogExit(returnValue: teams);
            return teams;
        }

        public async static Task InsertSingle(Teams team)
        {
            LogEntry(inputParams: team);

            await MobileServiceConnection.MobileService.GetTable<Teams>().InsertAsync(team);

            LogExit();
        }
    }
}
