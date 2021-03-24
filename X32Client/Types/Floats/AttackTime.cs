namespace Suhock.X32.Types.Floats
{
    public class AttackTime : LinearFloat
    {
        public override float MinUnitValue => 0.0f;

        public override float MaxUnitValue => 120.0f;

        public override float StepInterval => 1.0f;

        public override string Unit => "ms";

        private static AttackTime _minValue;

        private static AttackTime _maxValue;

        public static AttackTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static AttackTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected AttackTime() { }

        public AttackTime(float unitValue) : base(unitValue) { }

        public AttackTime(int stepValue) : base(stepValue) { }

        public static AttackTime FromEncodedValue(float encodedValue) =>
            new AttackTime() { EncodedValue = encodedValue };
    }
}
