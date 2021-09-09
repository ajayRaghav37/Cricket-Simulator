using System;
using System.Linq;
using Cricket_Simulator.Enums;
using static Logging.Logger;

namespace Cricket_Simulator.Logic
{
    public static class Utilities
    {
        static Random rnd = new Random();

        /// <summary>
        /// Gets a random result from a given set of events' likelihoods.
        /// </summary>
        /// <param name="comments">Comments for usage.</param>
        /// <param name="likelihoods">Set of relative likely occurrence of events.</param>
        /// <returns>Random event (0 based).</returns>
        public static int GetRandomResultFromLikelihoods(params int[] likelihoods)
        {
            LogEntry(inputParams: likelihoods);

            if (likelihoods == null)
                throw new ArgumentNullException(nameof(likelihoods));

            for (int i = 0; i < likelihoods?.Length; i++)
                if (likelihoods[i] < 0)
                    likelihoods[i] = 0;

            int result = rnd.Next(likelihoods.Sum());
            int progressiveSum = 0;

            for (int i = 0; i < likelihoods.Length; i++)
                if (result < likelihoods[i] + progressiveSum)
                {
                    LogExit(returnValue: i);
                    return i;
                }
                else
                    progressiveSum += likelihoods[i];

            throw new InvalidOperationException("Function didn't return a value.");
        }

        /// <summary>
        /// Returns a string that contains overs that are used to display in scoreboard.
        /// </summary>
        /// <param name="ballsBowled"></param>
        /// <param name="isCommentaryStyle"></param>
        /// <returns>Display overs string</returns>
        public static string GetOversString(int ballsBowled, bool isCommentaryStyle = false)
        {
            LogEntry(inputParams: new object[] { ballsBowled, isCommentaryStyle });

            string oversString;

            int overPart = (int)Math.Floor((double)ballsBowled / 6);
            string ballPart = "." + (ballsBowled % 6).ToString();
            if (ballPart == ".0")
            {
                if (isCommentaryStyle && ballsBowled > 0)
                {
                    ballPart = ".6";
                    overPart--;
                }
                else
                    ballPart = string.Empty;
            }

            oversString = overPart.ToString() + ballPart;

            LogExit(returnValue: oversString);
            return oversString;
        }

        public static string GetQuantifiedString(string item, int quantity)
        {
            LogEntry(inputParams: new object[] { item, quantity });

            string quantifiedString;

            if (quantity == 1)
                quantifiedString = quantity + " " + item;
            else
                quantifiedString = quantity + " " + item + "s";

            LogExit(returnValue: quantifiedString);
            return quantifiedString;
        }

        public static double GetRunRate(int runs, string overs)
        {
            LogEntry(inputParams: new object[] { runs, overs });

            double runRate = GetRunRate(runs, GetBallsNumber(overs)); ;

            LogExit(returnValue: runRate);
            return runRate;
        }

        public static double GetRunRate(double runs, double balls)
        {
            LogEntry(inputParams: new object[] { runs, balls });

            double runRate = 0;

            if (balls != 0)
                runRate = runs * 6 / balls;

            LogExit(returnValue: runRate);
            return runRate;
        }

        public static int GetBallsNumber(string overs)
        {
            LogEntry(inputParams: overs);

            double dblOvers = double.Parse(overs);

            int overPart = (int)dblOvers;
            int ballPart = (int)((dblOvers - overPart) * 10);

            int ballsNumber = overPart * 6 + ballPart;

            LogExit(returnValue: ballsNumber);
            return ballsNumber;
        }

        public static string GetExtraTypeNotation(ExtraType extraType)
        {
            LogEntry(inputParams: extraType);

            string extraTypeNotation;

            switch (extraType)
            {
                case ExtraType.Wide:
                    extraTypeNotation = "wd";
                    break;
                case ExtraType.LegBye:
                    extraTypeNotation = "lb";
                    break;
                case ExtraType.NoBall:
                    extraTypeNotation = "nb";
                    break;
                case ExtraType.Bye:
                    extraTypeNotation = "b";
                    break;
                case ExtraType.Penalty:
                    extraTypeNotation = "p";
                    break;
                default:
                    extraTypeNotation = string.Empty;
                    break;
            }

            LogExit(returnValue: extraTypeNotation);
            return extraTypeNotation;
        }
    }
}
