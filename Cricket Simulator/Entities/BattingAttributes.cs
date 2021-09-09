using static Logging.Logger;

namespace Cricket_Simulator.Entities
{
    public class BattingAttributes
    {
        //Instance attributes
        public int Runs { get; set; }
        public int BallsFaced { get; set; }
        public int DotBalls { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }
        public string Status { get; set; }
        public double StrikeRate
        {
            get
            {
                if (BallsFaced == 0)
                    return 0;

                return (double)Runs * 100 / BallsFaced;
            }
        }
        public double Aggression
        {
            get
            {
                return 1;
            }
        }

        public string Display
        {
            get
            {
                string display = Runs + "* (" + BallsFaced + "b";

                if (Fours > 0)
                    display += " " + Fours + "x4";

                if (Sixes > 0)
                    display += " " + Sixes + "x6";

                return display + ")";
            }
        }

        //Career attributes
        public double CareerStrikeRate { get; private set; }
        public double CareerAverage { get; private set; }
        public double CareerFourPercentage { get; private set; }
        public double CareerSixPercentage { get; private set; }

        private void InitializeMembers()
        {
            LogEntry();

            Runs = 0;
            BallsFaced = 0;
            DotBalls = 0;
            Fours = 0;
            Sixes = 0;

            LogExit();
        }

        public BattingAttributes()
        {
            LogEntry();

            InitializeMembers();

            LogExit();
        }

        public BattingAttributes(double careerStrikeRate, double careerAverage, double careerFourPercentage, double careerSixPercentage)
        {
            LogEntry(inputParams: new object[] { careerStrikeRate, careerAverage, careerFourPercentage, careerSixPercentage });

            InitializeMembers();
            CareerStrikeRate = careerStrikeRate;
            CareerAverage = careerAverage;
            CareerFourPercentage = careerFourPercentage;
            CareerSixPercentage = careerSixPercentage;

            LogExit();
        }
    }
}
