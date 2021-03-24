namespace Suhock.X32.Types.Floats
{
    public class QFactor : LogFloat
    {
        public override float MinUnitValue => 10;

        public override float MaxUnitValue => 0.3f;

        public override int Steps => 72;

        public override string Unit => "";
        
        public override string ToNodeString()
        {
            return ToFixedDecimalNodeString(1);
        }


        private static QFactor _minValue;

        private static QFactor _maxValue;

        public static QFactor MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static QFactor MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected QFactor() { }

        public QFactor(float unitValue) : base(unitValue) { }

        public QFactor(int stepValue) : base(stepValue) { }

        public static QFactor FromEncodedValue(float encodedValue) =>
            new QFactor() { EncodedValue = encodedValue };
    }
}
