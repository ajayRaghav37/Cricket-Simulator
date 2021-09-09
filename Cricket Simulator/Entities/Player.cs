namespace Cricket_Simulator.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string ShortName { get; set; }
        public string NickName { get; set; }
        public string CallingName { get; set; }
        public byte Role { get; set; }
        public BattingAttributes Batting { get; set; }
        public BowlingAttributes Bowling { get; set; }
        public FieldingAttributes Fielding { get; set; }
        public int Leadership { get; set; }
    }
}
