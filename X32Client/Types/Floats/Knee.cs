namespace Suhock.X32.Types.Floats
{
    public class Knee : LinearFloat
    {
        private static Knee _minValue;

        private static Knee _maxValue;

        public static Knee MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static Knee MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected Knee() { }

        public Knee(float unitValue) : base(unitValue) { }

        public Knee(int stepValue) : base(stepValue) { }

        public static Knee FromEncodedValue(float encodedValue) =>
            new Knee() { EncodedValue = encodedValue };

        public override float MinUnitValue => 0f;

        public override float MaxUnitValue => 5f;

        public override float StepInterval => 1f;

        public override string Unit => "";
    }
}
