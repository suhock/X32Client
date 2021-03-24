namespace Suhock.X32.Types.Floats
{
    public class MixPercent : LinearFloat
    {
        private static MixPercent _minValue;

        private static MixPercent _maxValue;

        public static MixPercent MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static MixPercent MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected MixPercent() { }

        public MixPercent(float unitValue) : base(unitValue) { }

        public MixPercent(int stepValue) : base(stepValue) { }

        public static MixPercent FromEncodedValue(float encodedValue) =>
            new MixPercent() { EncodedValue = encodedValue };

        public override float MinUnitValue => 0f;

        public override float MaxUnitValue => 100f;

        public override float StepInterval => 5f;

        public override string Unit => "%";
    }
}
