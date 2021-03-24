namespace Suhock.X32.Types.Floats
{
    public class Pan : LinearFloat
    {
        private static Pan _minValue;

        private static Pan _maxValue;

        public static Pan MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static Pan MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected Pan() { }

        public Pan(float unitValue) : base(unitValue) { }

        public Pan(int stepValue) : base(stepValue) { }

        public static Pan FromEncodedValue(float encodedValue) =>
            new Pan() { EncodedValue = encodedValue };

        public override float MinUnitValue => -100f;

        public override float MaxUnitValue => 100f;

        public override float StepInterval => 2f;

        public override string Unit => "%";
    }
}
