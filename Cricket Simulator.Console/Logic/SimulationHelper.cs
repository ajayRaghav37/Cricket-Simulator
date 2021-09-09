using System;
using System.Collections.Generic;
using Cricket_Simulator.Entities;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class SimulationHelper
    {
        public static double GetAggression(List<Player> battingPlayersRemaining, int ballsBowled)
        {
            LogEntry(inputParams: new object[] { battingPlayersRemaining, ballsBowled });

            if (battingPlayersRemaining == null)
                throw new ArgumentNullException(nameof(battingPlayersRemaining));

            double aggression;

            int totalBallsFuture = 0;

            for (int i = 0; i < battingPlayersRemaining.Count; i++)
            {
                int ballsPerPlayer = (int)(battingPlayersRemaining[i].Batting.CareerAverage * 100 / battingPlayersRemaining[i].Batting.CareerStrikeRate);

                if (battingPlayersRemaining[i].Batting.Status == null)
                    totalBallsFuture += ballsPerPlayer;
                else if (battingPlayersRemaining[i].Batting.Status == "not out")
                    if (ballsPerPlayer > battingPlayersRemaining[i].Batting.BallsFaced)
                        totalBallsFuture += ballsPerPlayer - battingPlayersRemaining[i].Batting.BallsFaced;
                    else
                        totalBallsFuture += ballsPerPlayer / 2;
            }

            aggression = totalBallsFuture * 0.8 / (120 - ballsBowled);

            if (ballsBowled < 36)
                aggression *= 1.2;
            else if (ballsBowled > 89)
                aggression *= 1.4;

            if (aggression < (double)1 / 2)
                aggression = (double)1 / 2;
            else if (aggression > 2)
                aggression = 2;

            LogExit(returnValue: aggression);
            return aggression;
        }

        public static void ChangeStrike(ref Player batsmanStrike, ref Player batsmanNonStrike)
        {
            LogEntry(inputParams: new object[] { batsmanStrike, batsmanNonStrike });

            Player temp = batsmanStrike;
            batsmanStrike = batsmanNonStrike;
            batsmanNonStrike = temp;

            LogExit();
        }

        public static string GetRoleString(byte role)
        {
            LogEntry(inputParams: role);

            string roleString = string.Empty;

            if (role >= 32)
            {
                roleString = "w" + roleString;
                role -= 32;
            }
            else if (role >= 16)
            {
                roleString = "W" + roleString;
                role -= 16;
            }

            if (role >= 8)
            {
                roleString = "o" + roleString;
                role -= 8;
            }
            else if (role >= 4)
            {
                roleString = "O" + roleString;
                role -= 4;
            }

            if (role >= 2)
            {
                roleString = "b" + roleString;
                role -= 2;
            }
            else if (role >= 1)
            {
                roleString = "B" + roleString;
                role -= 1;
            }

            LogExit(returnValue: roleString);
            return roleString;
        }
    }
}
