using Cricket_Simulator.Entities;
using static Logging.Logger;

namespace Cricket_Simulator.Logic
{
    public static class Commentary
    {
        public static string GetCommentary(BallResult ballResult)
        {
            LogEntry(inputParams: ballResult);

            string commentary = string.Empty;

            LogExit(returnValue: commentary);
            return commentary;
        }
    }
}
