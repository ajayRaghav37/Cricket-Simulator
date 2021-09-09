using Cricket_Simulator.Entities;

namespace Repositories.TableEntities
{
    public class Players : BaseEntity
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string ShortName { get; set; }
        public string NickName { get; set; }
        public string CallingName { get; set; }
        public byte Role { get; set; }
        public double BattingAverage { get; set; }
        public double BattingStrikeRate { get; set; }
        public double PercentRunsInFour { get; set; }
        public double PercentRunsInSix { get; set; }
        public double BowlingAverage { get; set; }
        public double BowlingEconomy { get; set; }

        public Player ToPlayer() => new Player
        {
            PlayerId = PlayerId,
            PlayerName = PlayerName,
            ShortName = ShortName,
            NickName = NickName,
            CallingName = CallingName,
            Role = Role,
            Batting = new BattingAttributes(BattingStrikeRate, BattingAverage, PercentRunsInFour, PercentRunsInSix),
            Bowling = new BowlingAttributes(BowlingEconomy, BowlingAverage)
        };
    }
}
