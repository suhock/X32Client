namespace Suhock.X32.Types.Floats
{
    public class FaderFineLevel : LevelFloat
    {
        private static FaderFineLevel _minValue;

        private static FaderFineLevel _maxValue;

        public static FaderFineLevel MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static FaderFineLevel MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected FaderFineLevel() { }

        public FaderFineLevel(float unitValue) : base(unitValue) { }

        public FaderFineLevel(int stepValue) : base(stepValue) { }

        public static FaderFineLevel FromEncodedValue(float encodedValue) =>
            new FaderFineLevel() { EncodedValue = encodedValue };

        public override int Steps => 1024;

        public override string ToNodeString()
        {
            return ToCompactNodeString(1);
        }
    }
}
