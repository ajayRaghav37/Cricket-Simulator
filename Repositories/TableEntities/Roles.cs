using Cricket_Simulator.Entities;

namespace Repositories.TableEntities
{
    public class Roles : BaseEntity
    {
        public int RoleId { get; set; }
        public string RoleDesc { get; set; }

        public Role ToRole() => new Role
        {
            RoleId = RoleId,
            RoleDesc = RoleDesc
        };
    }
}
