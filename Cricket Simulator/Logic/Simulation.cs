using System;
using System.Linq;
using Cricket_Simulator.Common;
using Cricket_Simulator.Entities;
using Cricket_Simulator.Enums;
using static Cricket_Simulator.Configuration.Likelihoods;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

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
            LogEntry(inputParams: new object[] { batsman, bowler, lastBallResult, conditions, pitch, aggression });

            if (bowler == null)
                throw new ArgumentNullException(nameof(bowler));

            if (batsman == null)
                throw new ArgumentNullException(nameof(batsman));

            BallResult ballResult = new BallResult();

            if (GetRandomResultFromLikelihoods(DEADBALL_OR_NOT) == 0)
                if (lastBallResult?.Wickets.WicketType != WicketType.None && GetRandomResultFromLikelihoods(TIMEDOUT_OR_NOT) == 0)
                {
                    ballResult.DeadBall = DeadBallType.TimedOut;
                    ballResult.Wickets.SetWicket(WicketType.TimedOut);
                }
                else
                {
                    ballResult.DeadBall = (DeadBallType)(GetRandomResultFromLikelihoods(DEADBALL_TYPE));
                    if (ballResult.DeadBall == DeadBallType.Mankaded)
                    {
                        ballResult.Wickets.SetWicket(WicketType.Mankaded, true);
                    }
                }
            else
            {
                switch (GetRandomResultFromLikelihoods(BALL_TYPE))
                {
                    case 0:
                        ballResult.Extras.Add(new Extra
                        {
                            ExtraType = ExtraType.Wide,
                            Runs = GetRandomResultFromLikelihoods(WIDE_RUNS)
                        });

                        bowler.Runs++;
                        bowler.WideBalls++;

                        if (ballResult.Extras.Last().Runs == 5)
                        {
                            bowler.Runs += 4;
                            bowler.WideBalls += 4;
                        }

                        ballResult.StrikeChange = (ballResult.Extras.Last().Runs % 2 == 0);

                        if (GetRandomResultFromLikelihoods(WIDE_WICKET_OR_NOT) == 0)
                        {
                            AddWicket(ballResult);

                            if (ballResult.Wickets.WicketType.In(WicketType.Stumped, WicketType.HitWicket))
                            {
                                ballResult.Extras.Last().Runs = 1;
                                ballResult.StrikeChange = false;
                            }
                        }
                        break;
                    case 1:
                        ballResult.Extras.Add(new Extra
                        {
                            ExtraType = ExtraType.NoBall,
                            Runs = 1
                        });
                        bowler.NoBalls++;
                        bowler.Runs++;
                        AddBattingAction(ballResult, batsman, bowler, aggression, pitch, conditions);
                        batsman.BallsFaced++;
                        if (ballResult.Runs == 0)
                            batsman.DotBalls++;
                        break;
                    case 2:
                        AddBattingAction(ballResult, batsman, bowler, aggression, pitch, conditions);
                        batsman.BallsFaced++;
                        bowler.Balls++;
                        if (ballResult.Runs == 0)
                        {
                            batsman.DotBalls++;
                            if (ballResult.Extras.Sum(item => item.Runs) == 0)
                                bowler.DotBalls++;
                        }
                        break;
                }
            }

            batsman.Runs += ballResult.Runs;

            bowler.Runs += ballResult.Runs;

            ballResult.Notation = GetNotation(ballResult);

            ballResult.Commentary = Commentary.GetCommentary(ballResult);

            LogExit(returnValue: ballResult);
            return ballResult;
        }

        private static void AddBattingAction(BallResult ballResult, BattingAttributes batsman, BowlingAttributes bowler, double aggression, double pitch, double conditions)
        {
            LogEntry(inputParams: new object[] { ballResult, batsman, bowler, aggression, pitch, conditions });

            if (GetRandomResultFromLikelihoods(EXTRAS_OR_NOT) == 0)
                AddExtras(ballResult);

            //Assuming 1000 balls are bowled, calculating likelihood of events.

            double runsByBatsman = batsman.StrikeRate * 10;
            double runsByBowler = bowler.CareerEconomy * (1000 / 6);
            double averageRuns = (runsByBatsman + runsByBowler) / 2;
            int runs = (int)(averageRuns * aggression * pitch * conditions);

            double averageRunsPerWicket = (batsman.CareerAverage + bowler.CareerAverage) / 2;
            int wickets = (int)((runs / averageRunsPerWicket) * (aggression / (pitch * conditions)));
            int runs4 = (int)((runs * batsman.CareerFourPercentage / 100) / 4);
            int runs6 = (int)((runs * batsman.CareerSixPercentage / 100) / 6);

            int runsRemaining = runs - (runs4 * 4 + runs6 * 6);

            int runs1 = runsRemaining * 61 / 100;
            int runs2 = (runsRemaining * 30 / 100) / 2;
            int runs3 = (runsRemaining * 9 / 100) / 3;
            int dots = 1000 - wickets - runs1 - runs2 - runs3 - runs4 - runs6;

            switch (GetRandomResultFromLikelihoods(wickets, runs1, runs2, runs3, runs4, runs6, dots))
            {
                case 0:
                    AddWicket(ballResult);

                    if (ballResult.Wickets.WicketType.In(WicketType.Bowled, WicketType.LBW, WicketType.Caught, WicketType.CaughtBehind, WicketType.Stumped, WicketType.HitWicket))
                        bowler.Wickets++;

                    break;
                case 1:
                    ballResult.Runs = 1;
                    ballResult.StrikeChange = true;
                    break;
                case 2:
                    ballResult.Runs = 2;
                    break;
                case 3:
                    ballResult.Runs = 3;
                    ballResult.StrikeChange = true;
                    break;
                case 4:
                    ballResult.Runs = 4;
                    batsman.Fours++;
                    break;
                case 5:
                    ballResult.Runs = 6;
                    batsman.Sixes++;
                    break;
                default:
                    break;
            }

            if (ballResult.Extras.Exists(item => item.ExtraType.In(ExtraType.Bye, ExtraType.LegBye, ExtraType.Penalty)))
                ballResult.Runs = 0;

            LogExit();
        }

        public static string GetNotation(BallResult ballResult)
        {
            LogEntry(inputParams: ballResult);

            string notation = string.Empty;

            if (ballResult.Runs > 0)
                notation = ballResult.Runs.ToString();

            foreach (Extra extra in ballResult.Extras)
            {
                switch (extra.ExtraType)
                {
                    case ExtraType.Bye:
                        notation += extra.Runs;
                        break;
                    case ExtraType.LegBye:
                        notation += extra.Runs;
                        break;
                    case ExtraType.Wide:
                        if (extra.Runs > 1)
                            notation += (extra.Runs - 1);
                        break;
                    case ExtraType.Penalty:
                        if (!string.IsNullOrEmpty(notation))
                            notation += "+";
                        notation += extra.Runs;
                        break;
                }

                notation += GetExtraTypeNotation(extra.ExtraType);

                if (ballResult.Extras.IndexOf(extra) < ballResult.Extras.Count - 1)
                    notation += "+";
            }

            if (ballResult.Wickets.WicketType != WicketType.None)
            {
                if (ballResult.Extras.Count > 0)
                    notation += "+";

                notation += "W";
            }

            if (string.IsNullOrEmpty(notation) && ballResult.DeadBall == DeadBallType.None)
                notation = "0";

            LogExit(returnValue: notation);
            return notation;
        }

        private static void AddExtras(BallResult ballResult)
        {
            LogEntry(inputParams: ballResult);

            switch (GetRandomResultFromLikelihoods(LEGAL_BALL_EXTRAS_TYPE))
            {
                case 0:
                    ballResult.Extras.Add(new Extra
                    {
                        ExtraType = ExtraType.LegBye,
                        Runs = GetRandomResultFromLikelihoods(LEGBYE_RUNS)
                    });

                    ballResult.StrikeChange = (ballResult.Extras.Last().Runs % 2 == 1);
                    break;
                case 1:
                    ballResult.Extras.Add(new Extra
                    {
                        ExtraType = ExtraType.Bye,
                        Runs = GetRandomResultFromLikelihoods(BYE_RUNS)
                    });

                    ballResult.StrikeChange = (ballResult.Extras.Last().Runs % 2 == 1);
                    break;
                case 2:
                    ballResult.Extras.Add(new Extra
                    {
                        ExtraType = ExtraType.Penalty,
                        Runs = 5
                    });

                    break;
            }

            LogExit();
        }

        private static void AddWicket(BallResult ballResult)
        {
            LogEntry(inputParams: ballResult);

            ballResult.Wickets.SetWicket();

            if (ballResult.Wickets.WicketType.In(WicketType.Runout, WicketType.ObstructedField))
                ballResult.Runs = GetRandomResultFromLikelihoods(RUNOUT_RUNS);

            if (ballResult.Extras.Count > 0)
                switch (ballResult.Extras.First().ExtraType)
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
                        if (ballResult.Wickets.WicketType.In(WicketType.Caught, WicketType.Bowled, WicketType.CaughtBehind, WicketType.HandledBall, WicketType.HitTwice, WicketType.LBW, WicketType.Mankaded, WicketType.Stumped))
                            ballResult.Wickets.WicketType = WicketType.None;
                        break;
                }

            if (ballResult.Wickets.WicketType == WicketType.Caught || ballResult.Wickets.WicketType == WicketType.ObstructedField || ballResult.Wickets.WicketType == WicketType.Runout)
                if (GetRandomResultFromLikelihoods(HEADS_OR_TAILS) == 0)
                    ballResult.StrikeChange = true;

            LogExit();
        }
    }
}
