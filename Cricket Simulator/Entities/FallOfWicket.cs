namespace Cricket_Simulator.Entities
{
    public class FallOfWicket
    {
        public int Runs { get; set; }
        public int WicketNumber { get; set; }
        public string Overs { get; set; }
        public string PlayerName { get; set; }

        public string Display
        {
            get
            {
                return WicketNumber + "-" + Runs + " (" + PlayerName + ", " + Overs + " ov)";
            }
        }
    }
}
