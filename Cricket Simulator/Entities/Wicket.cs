using Cricket_Simulator.Enums;
using static Cricket_Simulator.Configuration.Likelihoods;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

namespace Cricket_Simulator.Entities
{
    public class Wicket
    {
        public WicketType WicketType { get; set; }
        public bool IsNonStriker { get; set; }

        public void SetWicket()
        {
            LogEntry();

            WicketType = (WicketType)GetRandomResultFromLikelihoods(WICKET_TYPE);

            if (WicketType == WicketType.ObstructedField || WicketType == WicketType.Runout)
                if (GetRandomResultFromLikelihoods(HEADS_OR_TAILS) == 0)
                    IsNonStriker = true;

            LogExit();
        }

        public void SetWicket(WicketType wicketType, bool isNonStriker = false)
        {
            LogEntry(inputParams: new object[] { wicketType, isNonStriker });

            WicketType = wicketType;
            IsNonStriker = isNonStriker;

            LogExit();
        }
    }
}
