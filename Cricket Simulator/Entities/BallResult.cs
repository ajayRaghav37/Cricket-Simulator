using Cricket_Simulator.Enums;
namespace Cricket_Simulator.Entities
{
    public class BallResult
    {
        public DeadBallType DeadBall;
        public int Runs { get; set; }
        public Wicket Wickets { get; set; }
        public Extra Extras { get; set; }
        public string Notation { get; set; }
        public string Commentary { get; set; }

        public BallResult()
        {
            Wickets = new Wicket();
            Extras = new Extra();
            Notation = string.Empty;
            Commentary = string.Empty;
        }
    }
}
