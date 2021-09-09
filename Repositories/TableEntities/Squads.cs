using Cricket_Simulator.Entities;

namespace Repositories.TableEntities
{
    public class Squads : BaseEntity
    {
        public int TeamId { get; set; }
        public int PlayerId { get; set; }

        public Squad ToSquad() => new Squad
        {
            TeamId = TeamId,
            PlayerId = PlayerId
        };
    }
}
