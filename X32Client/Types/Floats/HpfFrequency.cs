namespace Suhock.X32.Types.Floats
{
    public class HpfFrequency : Frequency
    {
        public override float MinUnitValue => 20f;

        public override float MaxUnitValue => 400f;

        public override int Steps => 101;

        public override string ToNodeString()
        {
            return ToFixedDecimalNodeString(0);
        }


        private static HpfFrequency _minValue;

        private static HpfFrequency _maxValue;

        public static HpfFrequency MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static HpfFrequency MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected HpfFrequency() { }

        public HpfFrequency(float unitValue) : base(unitValue) { }

        public HpfFrequency(int stepValue) : base(stepValue) { }

        public static HpfFrequency FromEncodedValue(float encodedValue) =>
            new HpfFrequency() { EncodedValue = encodedValue };
    }
}
