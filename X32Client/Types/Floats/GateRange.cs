namespace Suhock.X32.Types.Floats
{
    public class GateRange : LinearFloat
    {
        private static GateRange _minValue;

        private static GateRange _maxValue;

        public static GateRange MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static GateRange MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected GateRange() { }

        public GateRange(float unitValue) : base(unitValue) { }

        public GateRange(int stepValue) : base(stepValue) { }

        public static GateRange FromEncodedValue(float encodedValue) =>
            new GateRange() { EncodedValue = encodedValue };

        public override float MinUnitValue => 3f;

        public override float MaxUnitValue => 60f;

        public override float StepInterval => 1.0f;

        public override string Unit => "dB";
    }
}
