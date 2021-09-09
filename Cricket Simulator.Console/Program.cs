using Cricket_Simulator.Entities;

namespace Cricket_Simulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSeveral(6, 4, 4, 1);
            TestSeveral(6, 4, 4, 2);
            TestSeveral(6, 4, 4, 3);
            TestSeveral(24, 10, 31, 1);
            TestSeveral(24, 10, 31, 2);
            TestSeveral(24, 10, 31, 3);
            TestSeveral(24, 10, 31, 4);
            TestSeveral(24, 10, 31, 5);
            TestSeveral(24, 10, 51, 10);
            TestSeveral(18, 10, 12, 1);
            TestSeveral(12, 4, 20, 1);
            TestSeveral(24, 12, 28, 2);
            TestSeveral(18, 6, 34, 0);

            //int totalRuns = 0;
            //int totalWickets = 0;

            //int totalBalls = 120;

            //BallResult ballResult = new BallResult();
            //for (int i = 0; i < totalBalls; i++)
            //{
            //    double aggression = GetAggression(totalWickets, totalBalls, i, totalRuns);

            //    ballResult = Simulation.GetNext(
            //        new BattingAttributes { Average = 40 - totalWickets * 3, FourPercentage = 40, SixPercentage = 10, StrikeRate = 150 - totalWickets * 5 },
            //        new BowlingAttributes { CareerAverage = 20, CareerEconomy = 7.5 },
            //        ballResult, 1, 1, aggression
            //        );

            //    totalRuns += ballResult.Runs + ballResult.Extras.Runs;
            //    if (ballResult.Wickets.WicketType != WicketType.None)
            //        totalWickets++;

            //    System.Console.Write(Utilities.GetNextBallOverString(i) + "\t" + ballResult.Notation);
            //    System.Console.WriteLine("\t" + totalRuns.ToString() + "/" + totalWickets.ToString() + "\t" + aggression);

            //    if (totalWickets == 10)
            //        break;

            //    if (ballResult.Extras.ExtraType.In(ExtraType.NoBall, ExtraType.Wide) || ballResult.DeadBall != DeadBallType.None)
            //        i--;
            //}

            System.Console.ReadKey();

            //List<int> results = new List<int>();
            //for (int i = 0; i < 10000; i++)
            //    results.Add(Cricket_Simulator.Logic.Utilities.GetRandomResultFromLikelihoods("Fatalties; Injuries; Safe", 1, 10, 100));

            //int count0 = results.Count(item => item == 0);
            //int count1 = results.Count(item => item == 1);
            //int count2 = results.Count(item => item == 2);
        }

        private static void TestSeveral(int balls, int dotBalls, int runs, int wickets)
        {
            BowlingAttributes bowlingAttributes = new BowlingAttributes();

            bowlingAttributes.Balls = balls;
            bowlingAttributes.DotBalls = dotBalls;
            bowlingAttributes.Runs = runs;
            bowlingAttributes.Wickets = wickets;

            System.Console.WriteLine(bowlingAttributes.Overs + "-" + bowlingAttributes.DotBalls + "-" + bowlingAttributes.Maidens + "-" + bowlingAttributes.Runs + "-" + bowlingAttributes.Wickets + "\t" + bowlingAttributes.Aggression);
        }

        private static double GetAggression(double totalWickets, double totalBalls, double ballsBowled, double runs)
        {
            double aggression;

            if (totalWickets > 0)
            {
                if (ballsBowled / totalWickets > totalBalls / 6)
                    aggression = 1 + (10 - totalWickets) / 10; //Do you agree with this one? (Y)
                else
                    aggression = 1 - totalWickets / 10; //(N)
            }
            else
                aggression = 1 + ballsBowled / totalBalls;

            if (ballsBowled * 10 / totalBalls < 3)
                aggression *= 1.2;
            else if (ballsBowled * 10 / totalBalls > 7.5)
                aggression *= 1.4;

            return aggression;
        }
    }
}
