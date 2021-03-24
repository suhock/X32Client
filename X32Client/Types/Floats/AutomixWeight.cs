namespace Suhock.X32.Types.Floats
{
    public class AutomixWeight : LinearFloat
    {
        private static AutomixWeight _minValue;

        private static AutomixWeight _maxValue;

        public static AutomixWeight MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static AutomixWeight MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected AutomixWeight() { }

        public AutomixWeight(float unitValue) : base(unitValue) { }

        public AutomixWeight(int stepValue) : base(stepValue) { }

        public static AutomixWeight FromEncodedValue(float encodedValue) =>
            new AutomixWeight() { EncodedValue = encodedValue };

        public override float MinUnitValue => -12f;

        public override float MaxUnitValue => 12f;

        public override float StepInterval => 0.5f;

        public override string Unit => "";
    }
}
