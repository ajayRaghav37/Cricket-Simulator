using System;
using System.Linq;

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
        public static int GetRandomResultFromLikelihoods(string comments, params int[] likelihoods)
        {
            for (int i = 0; i < likelihoods.Length; i++)
                if (likelihoods[i] < 0)
                    likelihoods[i] = 0;

            int result = rnd.Next(likelihoods.Sum() - 1);
            int progressiveSum = 0;

            for (int i = 0; i < likelihoods.Length; i++)
                if (result < likelihoods[i] + progressiveSum)
                    return i;
                else
                    progressiveSum += likelihoods[i];

            throw new Exception("Function didn't return a value.");
        }

        /// <summary>
        /// Returns a string that contains overs that are used to display in scoreboard.
        /// </summary>
        /// <param name="ballsBowled"></param>
        /// <param name="isCommentaryStyle"></param>
        /// <returns>Display overs string</returns>
        public static string GetOversString(int ballsBowled, bool isCommentaryStyle = false)
        {
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

            return oversString;
        }
    }
}
