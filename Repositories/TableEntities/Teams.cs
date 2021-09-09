using System;
using System.Drawing;
using Cricket_Simulator.Entities;

namespace Repositories.TableEntities
{
    public class Teams : BaseEntity
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string ShortName { get; set; }
        public int Captain { get; set; }
        public int ViceCaptain { get; set; }
        public Uri LogoUri { get; set; }
        public Uri FlagUri { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public bool IsCounty { get; set; }

        public Team ToTeam() => new Team
        {
            TeamId = TeamId,
            TeamName = TeamName,
            ShortName = ShortName,
            Captain = Captain,
            ViceCaptain = ViceCaptain,
            LogoUri = LogoUri,
            FlagUri = FlagUri,
            Color1 = Color1,
            Color2 = Color2,
            IsCounty = IsCounty
        };
    }
}
