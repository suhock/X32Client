namespace Suhock.X32.Types.Floats
{
    public class HoldTime : LogFloat
    {
        public override float MinUnitValue => 0.02f;

        public override float MaxUnitValue => 2000.0f;

        public override int Steps => 101;

        public override string Unit => "ms";

        public override string ToNodeString()
        {
            float unitValue = UnitValue;

            if (unitValue < 10.0f)
            {
                return ToFixedDecimalNodeString(2);
            }
            else if (unitValue < 100.0f)
            {
                return ToFixedDecimalNodeString(1);
            }
            else
            {
                return ToFixedDecimalNodeString(0);
            }
        }


        private static HoldTime _minValue;

        private static HoldTime _maxValue;

        public static HoldTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static HoldTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected HoldTime() { }

        public HoldTime(float unitValue) : base(unitValue) { }

        public HoldTime(int stepValue) : base(stepValue) { }

        public static HoldTime FromEncodedValue(float encodedValue) =>
            new HoldTime() { EncodedValue = encodedValue };
    }
}
