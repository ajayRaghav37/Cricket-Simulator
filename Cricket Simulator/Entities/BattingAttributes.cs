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
        public double StrikeRate
        {
            get
            {
                return Runs * 100 / BallsFaced;
            }
        }
        public double Aggression
        {
            get
            {
                return 1;
            }
        }

        //Career attributes
        public double CareerStrikeRate { get; private set; }
        public double CareerAverage { get; private set; }
        public double CareerFourPercentage { get; private set; }
        public double CareerSixPercentage { get; private set; }

        private void InitializeMembers()
        {
            Runs = 0;
            BallsFaced = 0;
            DotBalls = 0;
            Fours = 0;
            Sixes = 0;
        }

        public BattingAttributes()
        {
            InitializeMembers();
        }

        public BattingAttributes(double careerStrikeRate, double careerAverage, double careerFourPercentage, double careerSixPercentage)
        {
            InitializeMembers();
            CareerStrikeRate = careerStrikeRate;
            CareerAverage = careerAverage;
            CareerFourPercentage = careerFourPercentage;
            CareerSixPercentage = careerSixPercentage;
        }
    }
}
