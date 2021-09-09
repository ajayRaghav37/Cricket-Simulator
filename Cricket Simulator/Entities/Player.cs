namespace Cricket_Simulator.Entities
{
    public class Player
    {
        public string PlayerName { get; set; }
        public BattingAttributes Batting { get; set; }
        public BowlingAttributes Bowling { get; set; }
        public FieldingAttributes Fielding { get; set; }
        public int Leadership { get; set; }
    }
}
