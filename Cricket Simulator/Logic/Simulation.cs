using Cricket_Simulator.Common;
using Cricket_Simulator.Entities;
using Cricket_Simulator.Enums;

namespace Cricket_Simulator.Logic
{
    public static class Simulation
    {
        /// <summary>
        /// Gets result for next ball.
        /// </summary>
        /// <param name="batsman"></param>
        /// <param name="bowler"></param>
        /// <param name="conditions">Bowling conditions if less than 1, batting conditions if more than 1.</param>
        /// <param name="pitch">Bowling pitch if less than 1, batting pitch if more than 1.</param>
        /// <param name="aggression"></param>
        /// <returns></returns>
        public static BallResult GetNext(BattingAttributes batsman, BowlingAttributes bowler, BallResult lastBallResult, double conditions = 1, double pitch = 1, double aggression = 1)
        {
            BallResult ballResult = new BallResult();

            if (Utilities.GetRandomResultFromLikelihoods("Dead ball; Ball bowled", 1, 100) == 0)
                if (lastBallResult.Wickets.WicketType != WicketType.None && Utilities.GetRandomResultFromLikelihoods("Timed out; Not out", 1, 1000) == 0)
                {
                    ballResult.DeadBall = DeadBallType.TimedOut;
                    ballResult.Wickets.SetWicket(WicketType.TimedOut, false);
                }
                else
                {
                    ballResult.DeadBall = (DeadBallType)(2 + Utilities.GetRandomResultFromLikelihoods("Refer dead ball types from 3rd", 1, 5, 5, 20, 50, 1000));
                    if (ballResult.DeadBall == DeadBallType.Mankaded)
                        ballResult.Wickets.SetWicket(WicketType.Mankaded, true);
                }
            else
            {
                switch (Utilities.GetRandomResultFromLikelihoods("Wide ball; No ball; Legitimate ball", 10, 1, 250))
                {
                    case 0:
                        ballResult.Extras.ExtraType = ExtraType.Wide;
                        ballResult.Extras.Runs = 1 + Utilities.GetRandomResultFromLikelihoods("Runs with wide: 0; 1; 2; 3; 4", 500, 50, 25, 10, 50);

                        if (Utilities.GetRandomResultFromLikelihoods("Wicket on wide; No wicket on wide", 1, 150) == 0)
                        {
                            AddWicket(ballResult);
                            if (ballResult.Wickets.WicketType.In(WicketType.Bowled, WicketType.LBW, WicketType.Caught, WicketType.CaughtBehind, WicketType.HitTwice, WicketType.HandledBall))
                                ballResult.Wickets.WicketType = WicketType.None;

                            if (ballResult.Wickets.WicketType.In(WicketType.Stumped, WicketType.HitWicket))
                                ballResult.Extras.Runs = 1;
                        }
                        break;
                    case 1:
                        ballResult.Extras.ExtraType = ExtraType.NoBall;
                        ballResult.Extras.Runs = 1;
                        AddBattingAction(ballResult, batsman, bowler, aggression, pitch, conditions);
                        break;
                    case 2:
                        AddBattingAction(ballResult, batsman, bowler, aggression, pitch, conditions);
                        break;
                }
            }

            ballResult.Notation = GetNotation(ballResult);

            ballResult.Commentary = Commentary.GetCommentary(ballResult);

            return ballResult;
        }

        private static void AddBattingAction(BallResult ballResult, BattingAttributes batsman, BowlingAttributes bowler, double aggression, double pitch, double conditions)
        {
            //if (Utilities.GetRandomResultFromLikelihoods("Extras; Otherwise", 1, 25) == 0)
            //    AddExtras(ballResult);

            ////Assuming 1000 balls are bowled, calculating likelihood of events.

            //double runsByBatsman = batsman.StrikeRate * 10;
            //double runsByBowler = bowler.CareerEconomy * (1000 / 6);
            //double averageRuns = (runsByBatsman + runsByBowler) / 2;
            //int runs = (int)(averageRuns * aggression * pitch * conditions);

            //double averageRunsPerWicket = (batsman.Average + bowler.CareerAverage) / 2;
            //int wickets = (int)((runs / averageRunsPerWicket) * (aggression / (pitch * conditions)));
            //int runs4 = (int)((runs * batsman.FourPercentage / 100) / 4);
            //int runs6 = (int)((runs * batsman.SixPercentage / 100) / 6);

            //int runsRemaining = runs - (runs4 * 4 + runs6 * 6);

            //int runs1 = runsRemaining * 61 / 100;
            //int runs2 = (runsRemaining * 30 / 100) / 2;
            //int runs3 = (runsRemaining * 9 / 100) / 3;
            //int dots = 1000 - wickets - runs1 - runs2 - runs3 - runs4 - runs6;

            //switch (Utilities.GetRandomResultFromLikelihoods("Probability of a ball to have stated results", wickets, runs1, runs2, runs3, runs4, runs6, dots))
            //{
            //    case 0:
            //        AddWicket(ballResult);
            //        break;
            //    case 1:
            //        ballResult.Runs = 1;
            //        break;
            //    case 2:
            //        ballResult.Runs = 2;
            //        break;
            //    case 3:
            //        ballResult.Runs = 3;
            //        break;
            //    case 4:
            //        ballResult.Runs = 4;
            //        break;
            //    case 5:
            //        ballResult.Runs = 6;
            //        break;
            //}

            //if (ballResult.Extras.ExtraType.In(ExtraType.Bye, ExtraType.LegBye, ExtraType.Penalty))
            //    ballResult.Runs = 0;
        }

        private static string GetNotation(BallResult ballResult)
        {
            string notation = string.Empty;

            if (ballResult.Runs > 0)
                notation = ballResult.Runs.ToString();

            if (ballResult.Extras.ExtraType != ExtraType.None)
            {
                switch (ballResult.Extras.ExtraType)
                {
                    case ExtraType.Bye:
                        notation += ballResult.Extras.Runs.ToString() + "b";
                        break;
                    case ExtraType.LegBye:
                        notation += ballResult.Extras.Runs.ToString() + "lb";
                        break;
                    case ExtraType.Wide:
                        if (ballResult.Extras.Runs > 1)
                            notation += (ballResult.Extras.Runs - 1).ToString();
                        notation += "wd";
                        break;
                    case ExtraType.NoBall:
                        notation += "nb";
                        break;
                    case ExtraType.Penalty:
                        if (notation != string.Empty)
                            notation += "+";
                        notation += ballResult.Extras.Runs.ToString() + "p";
                        break;
                }

                if (ballResult.Wickets.WicketType != WicketType.None)
                    notation += "+";
            }

            if (ballResult.Wickets.WicketType != WicketType.None)
                notation += "W";

            if (notation == string.Empty && ballResult.DeadBall == DeadBallType.None)
                notation = "0";

            return notation;
        }

        private static void AddExtras(BallResult ballResult)
        {
            switch (Utilities.GetRandomResultFromLikelihoods("Leg bye; Bye; Penalty", 400, 100, 1))
            {
                case 0:
                    ballResult.Extras.ExtraType = ExtraType.LegBye;
                    ballResult.Extras.Runs = 1 + Utilities.GetRandomResultFromLikelihoods("Runs with leg bye: 1; 2; 3; 4", 500, 25, 10, 50);
                    break;
                case 1:
                    ballResult.Extras.ExtraType = ExtraType.Bye;
                    ballResult.Extras.Runs = 1 + Utilities.GetRandomResultFromLikelihoods("Runs with byes: 1; 2; 3; 4", 500, 25, 10, 50);
                    break;
                case 2:
                    ballResult.Extras.ExtraType = ExtraType.Penalty;
                    ballResult.Extras.Runs = 5;
                    break;
            }
        }

        private static void AddWicket(BallResult ballResult)
        {
            ballResult.Wickets.SetWicket();

            if (ballResult.Extras.ExtraType == ExtraType.None)
            {
                if (ballResult.Wickets.WicketType.In(WicketType.Runout, WicketType.ObstructedField))
                    ballResult.Runs = Utilities.GetRandomResultFromLikelihoods("Runouts while taking runs: 1st; 2nd; 3rd; 4th", 60, 30, 9, 1);
            }
            else
                switch (ballResult.Extras.ExtraType)
                {
                    case ExtraType.NoBall:
                        if (ballResult.Wickets.WicketType.In(WicketType.Bowled, WicketType.LBW, WicketType.Caught, WicketType.CaughtBehind, WicketType.Stumped, WicketType.HitWicket))
                            ballResult.Wickets.WicketType = WicketType.None;
                        break;
                    case ExtraType.Wide:
                        if (ballResult.Wickets.WicketType.In(WicketType.Bowled, WicketType.LBW, WicketType.Caught, WicketType.CaughtBehind, WicketType.HitTwice))
                            ballResult.Wickets.WicketType = WicketType.None;
                        break;
                    default:
                        if (ballResult.Wickets.WicketType.In(WicketType.Caught, WicketType.Bowled, WicketType.CaughtBehind, WicketType.HandledBall, WicketType.HitTwice, WicketType.LBW, WicketType.LBW, WicketType.Mankaded, WicketType.Stumped))
                            ballResult.Wickets.WicketType = WicketType.None;
                        break;
                }
        }
    }
}
