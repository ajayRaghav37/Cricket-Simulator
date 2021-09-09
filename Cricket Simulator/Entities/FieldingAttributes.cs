using Cricket_Simulator.Enums;

namespace Cricket_Simulator.Entities
{
    public class FieldingAttributes
    {
        public double FieldingSkill { get; set; }
        public FieldingPosition PrimaryPosition { get; set; }
        public FieldingPosition SecondaryPosition { get; set; }
        public FieldingPosition CurrentPosition { get; set; }
    }
}
