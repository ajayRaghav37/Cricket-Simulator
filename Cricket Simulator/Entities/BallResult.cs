using System.Collections.Generic;
using Cricket_Simulator.Enums;
using static Logging.Logger;

namespace Cricket_Simulator.Entities
{
    public class BallResult
    {
        public DeadBallType DeadBall { get; set; }
        public int Runs { get; set; }
        public Wicket Wickets { get; set; }
        public List<Extra> Extras { get; }
        public bool StrikeChange { get; set; }
        public string Notation { get; set; }
        public string Commentary { get; set; }

        public BallResult()
        {
            LogEntry();
            Wickets = new Wicket();
            Extras = new List<Extra>();
            Notation = string.Empty;
            Commentary = string.Empty;
            StrikeChange = false;
            LogExit();
        }
    }
}
