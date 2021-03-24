namespace Suhock.X32.Types.Floats
{
    public class DynamicsThreshold : LinearFloat
    {
        private static DynamicsThreshold _minValue;

        private static DynamicsThreshold _maxValue;

        public static DynamicsThreshold MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static DynamicsThreshold MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected DynamicsThreshold() { }

        public DynamicsThreshold(float unitValue) : base(unitValue) { }

        public DynamicsThreshold(int stepValue) : base(stepValue) { }

        public static DynamicsThreshold FromEncodedValue(float encodedValue) =>
            new DynamicsThreshold() { EncodedValue = encodedValue };

        public override float MinUnitValue => -60.0f;

        public override float MaxUnitValue => 0.0f;

        public override float StepInterval => 0.5f;

        public override string Unit => "dB";
    }
}
