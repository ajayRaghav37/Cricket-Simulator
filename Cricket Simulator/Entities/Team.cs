using System;
using System.Drawing;

namespace Cricket_Simulator.Entities
{
    public class Team
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
    }
}
