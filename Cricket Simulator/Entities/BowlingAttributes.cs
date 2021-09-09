using System;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

namespace Cricket_Simulator.Entities
{
    public class BowlingAttributes
    {
        //Private fields
        int balls;
        string overs;

        //Instance attributes
        public int Balls
        {
            get
            {
                return balls;
            }
            set
            {
                balls = value;
                overs = GetOversString(value);
            }
        }
        public string Overs
        {
            get
            {
                return overs;
            }
        }
        public int Maidens { get; set; }
        public int DotBalls { get; set; }
        public int Runs { get; set; }
        public int Wickets { get; set; }
        public int NoBalls { get; set; }
        public int WideBalls { get; set; }
        public double Economy
        {
            get
            {
                return GetRunRate(Runs, Balls);
            }
        }
        public double Aggression
        {
            get
            {
                //Use economy [1] (oppose), wickets [3] (favor) and dot balls [2] (favor)

                double aggression = 1;

                if (Balls > 0)
                {
                    double strikingFactor = (Wickets * 6 + DotBalls) / (double)Balls;

                    if (strikingFactor >= 1)
                        aggression = Math.Pow(strikingFactor, 0.5);
                    else
                        aggression = 2 - Math.Pow(2 - strikingFactor, 0.5);
                }

                return aggression;
            }
        }

        public string Display
        {
            get
            {
                return Overs + "-" + Maidens + "-" + DotBalls + "-" + Runs + "-" + Wickets;
            }
        }

        //Career attributes
        public double CareerEconomy { get; private set; }
        public double CareerAverage { get; private set; }
        public double CareerStrikeRate
        {
            get
            {
                return GetRunRate(CareerAverage, CareerEconomy);
            }
        }

        private void InitializeMembers()
        {
            LogEntry();

            balls = 0;
            Runs = 0;
            overs = "0";
            Wickets = 0;
            Maidens = 0;
            DotBalls = 0;

            LogExit();
        }

        public BowlingAttributes()
        {
            LogEntry();

            InitializeMembers();

            LogExit();
        }

        public BowlingAttributes(double careerEconomy, double careerAverage)
        {
            LogEntry(inputParams: new object[] { careerEconomy, careerAverage });

            InitializeMembers();
            CareerAverage = careerAverage;
            CareerEconomy = careerEconomy;

            LogExit();
        }
    }
}
