namespace Suhock.X32.Types.Floats
{
    public class MakeupGain : LinearFloat
    {
        public override float MinUnitValue => 0f;

        public override float MaxUnitValue => 24f;

        public override float StepInterval => 0.5f;

        public override string Unit => "dB";

        private static MakeupGain _minValue;

        private static MakeupGain _maxValue;

        public static MakeupGain MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static MakeupGain MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected MakeupGain() { }

        public MakeupGain(float unitValue) : base(unitValue) { }

        public MakeupGain(int stepValue) : base(stepValue) { }

        public static MakeupGain FromEncodedValue(float encodedValue) =>
            new MakeupGain() { EncodedValue = encodedValue };
    }
}
