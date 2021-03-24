namespace Suhock.X32.Types.Floats
{
    public abstract class Frequency : LogFloat
    {
        public override string Unit => "Hz";

        public override string ToNodeString()
        {
            float unitValue = UnitValue;

            if (unitValue < 1000f)
            {
                return ToFixedDecimalNodeString(1);
            }
            else
            {
                return ((int)unitValue / 1000) + "k" + ((int)unitValue % 1000 / 10);
            }
        }


        protected Frequency() { }

        public Frequency(float unitValue) : base(unitValue) { }

        public Frequency(int stepValue) : base(stepValue) { }
    }
}
