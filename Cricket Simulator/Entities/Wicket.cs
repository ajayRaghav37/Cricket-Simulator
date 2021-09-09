using Cricket_Simulator.Enums;
using Cricket_Simulator.Logic;

namespace Cricket_Simulator.Entities
{
    public class Wicket
    {
        public WicketType WicketType { get; set; }
        public bool StrikeChange { get; set; }

        public void SetWicket()
        {
            WicketType = (WicketType)Utilities.GetRandomResultFromLikelihoods("Refer wicket types enum", 0, 0, 0, 1, 100, 100, 200, 500, 20000, 50000, 200000, 200000, 250000, 1000000);

            if (WicketType == WicketType.Caught || WicketType == WicketType.ObstructedField || WicketType == WicketType.Runout)
                if (Utilities.GetRandomResultFromLikelihoods("Equally likely", 1, 1) == 0)
                    StrikeChange = true;
        }

        public void SetWicket(WicketType wicketType, bool strikeChange = false)
        {
            WicketType = wicketType;
            StrikeChange = strikeChange;
        }
    }
}
