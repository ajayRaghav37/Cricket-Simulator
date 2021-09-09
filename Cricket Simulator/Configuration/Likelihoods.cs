namespace Cricket_Simulator.Configuration
{
    /// <summary>
    /// Contains constant arrays that contain likelihoods.
    /// </summary>
    public static class Likelihoods
    {
        //Caught by bowler; Caught by other
        public static readonly int[] CAUGHT_BY_BOWLER_OR_NOT = new int[] { 1, 30 };

        //Dead ball; Ball bowled
        public static readonly int[] DEADBALL_OR_NOT = new int[] { 1, 100 };

        //Extras; Otherwise
        public static readonly int[] EXTRAS_OR_NOT = new int[] { 1, 25 };

        //Heads; Tails
        public static readonly int[] HEADS_OR_TAILS = new int[] { 1, 1 };

        //Timed out; Not out
        public static readonly int[] TIMEDOUT_OR_NOT = new int[] { 1, 1000 };

        //Wicket on wide; No wicket on wide
        public static readonly int[] WIDE_WICKET_OR_NOT = new int[] { 1, 150 };


        //Wide ball; No ball; Legitimate ball
        public static readonly int[] BALL_TYPE = new int[] { 10, 1, 250 };

        //Refer dead ball types enum
        public static readonly int[] DEADBALL_TYPE = new int[] { 0, 0, 1, 5, 5, 20, 50, 1000 };

        //Leg bye; Bye; Penalty
        public static readonly int[] LEGAL_BALL_EXTRAS_TYPE = new int[] { 400, 100, 1 };

        //Direct hit; Bowler assisted; Random fielders; Wicketkeeper assisted
        public static readonly int[] RUNOUT_TYPE = new int[] { 1, 1, 1, 2 };

        //Refer wicket types enum
        public static readonly int[] WICKET_TYPE = new int[] { 0, 0, 0, 1, 100, 100, 200, 500, 20000, 50000, 200000, 200000, 250000, 1000000 };


        //Runs with bye
        public static readonly int[] BYE_RUNS = new int[] { 0, 500, 25, 10, 50 };

        //Runs with leg bye
        public static readonly int[] LEGBYE_RUNS = new int[] { 0, 500, 25, 10, 50 };

        //Runout runs
        public static readonly int[] RUNOUT_RUNS = new int[] { 60, 30, 9, 1 };

        //Runs with wide
        public static readonly int[] WIDE_RUNS = new int[] { 0, 500, 50, 25, 10, 50 };
    }
}
