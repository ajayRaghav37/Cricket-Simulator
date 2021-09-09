using System;
using System.Collections.Generic;
using System.Linq;
using Cricket_Simulator.Common;
using Cricket_Simulator.Entities;
using Cricket_Simulator.Enums;
using Repositories.TableEntities;
using static System.Console;
using static Cricket_Simulator.Configuration.Likelihoods;
using static Cricket_Simulator.Console.ConsoleHelper;
using static Cricket_Simulator.Console.Logic.DisplayAssistant;
using static Cricket_Simulator.Console.Logic.SimulationHelper;
using static Cricket_Simulator.Logic.Simulation;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class FirstInnings
    {
        static bool freeHit = false;

        public static int Start(List<Teams> teams, List<Squads> squads, List<Players> players, int battingTeamIndex, int checkPoint)
        {
            LogEntry(inputParams: new object[] { teams, squads, players, battingTeamIndex, checkPoint });

            int[] columnWidths = GetColumnWidths(5, 0, 10);

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            if (teams == null)
                throw new ArgumentNullException(nameof(teams));

            WriteLine();

            List<Player> battingPlayers = GetPlayersForTeam(teams[battingTeamIndex].TeamId, players, squads);
            List<Player> fieldingPlayers = GetPlayersForTeam(teams[1 - battingTeamIndex].TeamId, players, squads);
            List<Player> bowlingPlayers = fieldingPlayers.FindAll(item => GetRoleString(item.Role).ToLower().Contains("o"));

            Player bowler = new Player();
            Player lastBowler = null;

            bowler = bowlingPlayers.FindAll(item => item.PlayerId != bowler.PlayerId)[GetRandomResultFromLikelihoods(Enumerable.Repeat(1, bowlingPlayers.FindAll(item => item.PlayerId != bowler.PlayerId).Count).ToArray())];

            Player batsmanStrike = battingPlayers[0];
            batsmanStrike.Batting.Status = "not out";
            Player batsmanNonStrike = battingPlayers[1];
            batsmanNonStrike.Batting.Status = "not out";

            int totalRuns = 0;
            int totalWickets = 0;

            BallResult ballResult = new BallResult();

            int overNumber = 1;
            int ballsInCurrentOver = 0;
            int totalRunsBeforeThisOver = 0;
            int totalWicketsBeforeThisOver = 0;

            List<Extra> extras = new List<Extra>();
            List<FallOfWicket> fallOfWickets = new List<FallOfWicket>();
            List<Player> bowlers = new List<Player> { bowler };
            List<Player> batsmen = new List<Player> { batsmanStrike, batsmanNonStrike };

            for (int i = 0; i < 120; i++)
            {
                double aggression = GetAggression(battingPlayers.FindAll(item => item.Batting.Status == null || item.Batting.Status == "not out"), i);
                bool isAllOut = false;

                RandomBallResult(columnWidths, bowler, batsmanStrike, ref totalRuns, ref totalWickets, ref ballResult, ref ballsInCurrentOver, extras, ref i, aggression, ref isAllOut);

                if (ballResult.Wickets.WicketType != WicketType.None)
                {
                    if (ballResult.Wickets.IsNonStriker)
                        SetNewBatsman(battingPlayers, ref batsmanNonStrike, fieldingPlayers, bowler, totalRuns, totalWickets, ballResult.Wickets.WicketType, fallOfWickets, batsmen, i + 1, isAllOut, checkPoint);
                    else
                        SetNewBatsman(battingPlayers, ref batsmanStrike, fieldingPlayers, bowler, totalRuns, totalWickets, ballResult.Wickets.WicketType, fallOfWickets, batsmen, i + 1, isAllOut, checkPoint);
                }

                ChangeStrike(checkPoint, ref batsmanStrike, ref batsmanNonStrike, ballResult, isAllOut);

                if (ballsInCurrentOver == 6 || isAllOut)
                {
                    DisplayEndOfOver(batsmanStrike, batsmanNonStrike, bowler, lastBowler, teams[battingTeamIndex].ShortName, totalRuns, totalWickets, overNumber, ballsInCurrentOver, totalRunsBeforeThisOver, totalWicketsBeforeThisOver, columnWidths);

                    if (isAllOut)
                        break;

                    bowler = GetNewBowler(checkPoint, bowlingPlayers, bowler, ref lastBowler, ref batsmanStrike, ref batsmanNonStrike, totalRuns, totalWickets, ref overNumber, ref ballsInCurrentOver, ref totalRunsBeforeThisOver, ref totalWicketsBeforeThisOver, bowlers);
                }
            }

            batsmen.AddRange(battingPlayers.FindAll(item => !batsmen.Contains(item)));

            DisplayScoreCard(teams, battingTeamIndex, batsmen, bowlers, extras, fallOfWickets);

            LogExit(returnValue: totalRuns);
            return totalRuns;
        }

        private static List<Player> GetPlayersForTeam(int teamId, List<Players> players, List<Squads> squads)
        {
            LogEntry(inputParams: new object[] { teamId, players, squads });

            List<Player> playersInTeam = new List<Player>();

            players.FindAll(p => squads.FindAll(s => s.TeamId == teamId).Select(s => s.PlayerId).Contains(p.PlayerId)).ForEach(item => playersInTeam.Add(item.ToPlayer()));

            LogExit(returnValue: playersInTeam);
            return playersInTeam;
        }

        private static void ChangeStrike(int checkPoint, ref Player batsmanStrike, ref Player batsmanNonStrike, BallResult ballResult, bool isAllOut)
        {
            LogEntry(inputParams: new object[] { checkPoint, batsmanStrike, batsmanNonStrike, ballResult, isAllOut });

            if (ballResult.StrikeChange && !isAllOut)
                SimulationHelper.ChangeStrike(ref batsmanStrike, ref batsmanNonStrike);

            if (checkPoint < 1)
                ReadKey();

            LogExit();
        }

        private static void RandomBallResult(int[] columnWidths, Player bowler, Player batsmanStrike, ref int totalRuns, ref int totalWickets, ref BallResult ballResult, ref int ballsInCurrentOver, List<Extra> extras, ref int i, double aggression, ref bool isAllOut)
        {
            LogEntry(inputParams: new object[] { columnWidths, bowler, batsmanStrike, totalRuns, totalWickets, ballResult, ballsInCurrentOver, extras, i, aggression, isAllOut });

            if (freeHit)
                aggression *= 3;

            ballResult = GetNext(
                batsmanStrike.Batting,
                bowler.Bowling,
                ballResult, 1, 1, aggression
                );

            if (freeHit)
            {
                if (ballResult.Wickets.WicketType.In(WicketType.Bowled, WicketType.LBW, WicketType.Caught, WicketType.CaughtBehind, WicketType.Stumped, WicketType.HitWicket))
                {
                    if (ballResult.StrikeChange)
                        ballResult.Runs = 1;

                    ballResult.Wickets.WicketType = WicketType.None;
                    ballResult.Wickets.IsNonStriker = false;
                    ballResult.Notation = GetNotation(ballResult);
                }

                if (ballResult.Extras.Count == 0 || ballResult.Extras.First().ExtraType.NotIn(ExtraType.NoBall, ExtraType.Wide) == true)
                    freeHit = false;
            }
            else
            {
                if (ballResult.Extras.Count > 0 && ballResult.Extras.First().ExtraType == ExtraType.NoBall)
                    freeHit = true;
            }

            extras.AddRange(ballResult.Extras);

            totalRuns += ballResult.Runs + ballResult.Extras.Sum(item => item.Runs);

            if (ballResult.Wickets.WicketType != WicketType.None)
            {
                totalWickets++;
                isAllOut = (totalWickets == 10);
            }

            Write(GetOversString(i + 1, true).PadRight(columnWidths[0]) + (bowler.CallingName + " to " + batsmanStrike.CallingName).PadRight(columnWidths[1]) + ballResult.Notation.PadLeft(columnWidths[2]));

            SetColorByAggression(aggression);

            Write(string.Format("{0:0.00}", Math.Round(aggression, 2)).PadLeft(6));

            WriteLine();

            if (ballResult.Wickets.WicketType != WicketType.None)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(string.Empty.PadRight(columnWidths[0]) + ballResult.Wickets.WicketType + (ballResult.Wickets.IsNonStriker ? ", Non-striker" : ", Striker"));
            }

            if (freeHit)
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine(string.Empty.PadRight(columnWidths[0]) + "Free hit");
            }

            ResetColor();

            if (ballResult.Extras.Exists(item => item.ExtraType.In(ExtraType.NoBall, ExtraType.Wide)) || ballResult.DeadBall != DeadBallType.None)
                i--;
            else
                ballsInCurrentOver++;

            LogExit();
        }

        private static Player GetNewBowler(int checkPoint, List<Player> bowlingPlayers, Player bowler, ref Player lastBowler, ref Player batsmanStrike, ref Player batsmanNonStrike, int totalRuns, int totalWickets, ref int overNumber, ref int ballsInCurrentOver, ref int totalRunsBeforeThisOver, ref int totalWicketsBeforeThisOver, List<Player> bowlers)
        {
            LogEntry(inputParams: new object[] { checkPoint, bowlingPlayers, bowler, lastBowler, batsmanStrike, batsmanNonStrike, totalRuns, totalWickets, overNumber, ballsInCurrentOver, totalRunsBeforeThisOver, totalWicketsBeforeThisOver, bowlers });

            Player newBowler = null;

            if (checkPoint < 2)
                ReadKey();

            if (overNumber < 20)
            {
                SimulationHelper.ChangeStrike(ref batsmanStrike, ref batsmanNonStrike);

                ballsInCurrentOver = 0;
                overNumber++;
                totalRunsBeforeThisOver = totalRuns;
                totalWicketsBeforeThisOver = totalWickets;

                lastBowler = bowler;
                newBowler = bowlingPlayers.FindAll(item => item.PlayerId != bowler.PlayerId && item.Bowling.Balls < 24)[GetRandomResultFromLikelihoods(Enumerable.Repeat(1, bowlingPlayers.Count(item => item.PlayerId != bowler.PlayerId && item.Bowling.Balls < 24)).ToArray())];

                if (!bowlers.Contains(newBowler))
                    bowlers.Add(newBowler);
            }

            LogExit(returnValue: newBowler);

            return newBowler;
        }

        private static void SetNewBatsman(List<Player> battingPlayers, ref Player batsman, List<Player> fieldingPlayers, Player bowler, int totalRuns, int totalWickets, WicketType wicketType, List<FallOfWicket> fallOfWickets, List<Player> batsmen, int balls, bool isAllOut, int checkPoint)
        {
            LogEntry(inputParams: new object[] { battingPlayers, batsman, fieldingPlayers, bowler, totalRuns, totalWickets, wicketType, fallOfWickets, batsmen, balls, isAllOut, checkPoint });

            batsman.Batting.Status = GetStatus(wicketType, bowler, fieldingPlayers.Find(item => GetRoleString(item.Role).Contains("W")) ?? fieldingPlayers.Find(item => GetRoleString(item.Role).Contains("w")), fieldingPlayers);

            fallOfWickets.Add(new FallOfWicket
            {
                Overs = GetOversString(balls, true),
                PlayerName = batsman.CallingName,
                Runs = totalRuns,
                WicketNumber = totalWickets
            });

            if (!isAllOut && balls < 120)
            {
                batsman = battingPlayers[totalWickets + 1];
                batsmen.Add(batsman);
                batsman.Batting.Status = "not out";
            }
            else
                batsman = null;

            if (checkPoint < 3)
                ReadKey();

            LogExit();
        }

        private static string GetStatus(WicketType wicketType, Player bowler, Player wicketKeeper, List<Player> fieldingPlayers)
        {
            LogEntry(inputParams: new object[] { wicketType, bowler, wicketKeeper, fieldingPlayers });

            string statusToReturn;

            switch (wicketType)
            {
                case WicketType.Mankaded:
                    statusToReturn = "runout (" + bowler.CallingName + ")";
                    break;
                case WicketType.TimedOut:
                    statusToReturn = "timed out";
                    break;
                case WicketType.HitTwice:
                    statusToReturn = "hit the ball twice";
                    break;
                case WicketType.HandledBall:
                    statusToReturn = "handled the ball";
                    break;
                case WicketType.ObstructedField:
                    statusToReturn = "obstructing the field";
                    break;
                case WicketType.HitWicket:
                    statusToReturn = "hit wicket b " + bowler.CallingName;
                    break;
                case WicketType.RetiredHurt:
                    statusToReturn = "retired hurt";
                    break;
                case WicketType.Stumped:
                    statusToReturn = "st " + wicketKeeper.CallingName + " b " + bowler.CallingName;
                    break;
                case WicketType.Runout:
                    string status = "runout (";

                    Player firstCredit = fieldingPlayers.ElementAt(GetRandomResultFromLikelihoods(Enumerable.Repeat(1, 11).ToArray()));

                    status += firstCredit.CallingName;

                    switch (GetRandomResultFromLikelihoods(RUNOUT_TYPE))
                    {
                        case 1:
                            if (firstCredit != bowler)
                                status += "/" + bowler.CallingName;
                            break;
                        case 2:
                            status += "/" + fieldingPlayers.Except(new List<Player> { firstCredit }).ElementAt(GetRandomResultFromLikelihoods(Enumerable.Repeat(1, 10).ToArray())).CallingName;
                            break;
                        case 3:
                            if (firstCredit != wicketKeeper)
                                status += "/" + wicketKeeper.CallingName;
                            break;
                        default:
                            break;
                    }

                    statusToReturn = status + ")";
                    break;
                case WicketType.CaughtBehind:
                    statusToReturn = "c " + wicketKeeper.CallingName + " b " + bowler.CallingName;
                    break;
                case WicketType.LBW:
                    statusToReturn = "lbw " + bowler.CallingName;
                    break;
                case WicketType.Bowled:
                    statusToReturn = "b " + bowler.CallingName;
                    break;
                case WicketType.Caught:
                    if (GetRandomResultFromLikelihoods(CAUGHT_BY_BOWLER_OR_NOT) == 0)
                        statusToReturn = "c & b " + bowler.CallingName;
                    else
                        statusToReturn = "c " + fieldingPlayers.Except(new List<Player> { bowler, wicketKeeper }).ElementAt(GetRandomResultFromLikelihoods(Enumerable.Repeat(1, 9).ToArray())).CallingName + " b " + bowler.CallingName;
                    break;
                default:
                    statusToReturn = string.Empty;
                    break;
            }

            LogExit(returnValue: statusToReturn);
            return statusToReturn;
        }

        private static void SetColorByAggression(double aggression)
        {
            LogEntry(inputParams: aggression);

            if (aggression > 1.5)
                ForegroundColor = ConsoleColor.DarkRed;
            else if (aggression > 1)
                ForegroundColor = ConsoleColor.DarkYellow;
            else
                ForegroundColor = ConsoleColor.DarkGreen;

            LogExit();
        }
    }
}
