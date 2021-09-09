using System;

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
                overs = Cricket_Simulator.Logic.Utilities.GetOversString(value);
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
        public double Economy
        {
            get
            {
                return Runs * 6 / (double)Balls;
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

                    //double economyDiff = (8 - Economy) / 8;

                    //if (Math.Abs(economyDiff) >= 1)
                    //    aggression += Math.Sign(economyDiff) * Math.Pow(Math.Abs(economyDiff), 0.1) / 4;
                    //else
                    //    aggression += Math.Sign(economyDiff) * Math.Pow(Math.Abs(economyDiff), 2) / 4;
                }

                return aggression;
            }
        }

        //Career attributes
        public double CareerEconomy { get; private set; }
        public double CareerAverage { get; private set; }
        public double CareerStrikeRate
        {
            get
            {
                return (CareerAverage * 6 / CareerEconomy);
            }
        }

        private void InitializeMembers()
        {
            balls = 0;
            Runs = 0;
            overs = "0";
            Wickets = 0;
            Maidens = 0;
            DotBalls = 0;
        }

        public BowlingAttributes()
        {
            InitializeMembers();
        }

        public BowlingAttributes(double careerEconomy, double careerAverage)
        {
            InitializeMembers();
            CareerAverage = careerAverage;
            CareerEconomy = careerEconomy;
        }
    }
}
