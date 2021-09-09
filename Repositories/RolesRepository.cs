using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Logging.Logger;
using Repositories.TableEntities;

namespace Repositories
{
    public static class RolesRepository
    {
        public async static void GetSingle(Roles role)
        {
            LogEntry(inputParams: role);

            List<Roles> roles = await MobileServiceConnection.MobileService.GetTable<Roles>().Where(item => item.RoleId == role.RoleId).ToListAsync();
            role = roles.First();

            LogExit();
        }

        public async static void GetAll(List<Roles> roles)
        {
            LogEntry(inputParams: roles);

            List<int> roleIds = roles.Select(item => item.RoleId).ToList();
            roles = await MobileServiceConnection.MobileService.GetTable<Roles>().Where(item => roleIds.Contains(item.RoleId)).ToListAsync();

            LogExit();
        }

        public async static Task InsertSingle(Roles role)
        {
            LogEntry(inputParams: role);

            await MobileServiceConnection.MobileService.GetTable<Roles>().InsertAsync(role);

            LogExit();
        }
    }
}
