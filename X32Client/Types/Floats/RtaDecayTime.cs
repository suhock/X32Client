﻿namespace Suhock.X32.Types.Floats
{
    public class RtaDecayTime : LogFloat
    {
        public override float MinUnitValue => 0.25f;

        public override float MaxUnitValue => 16.0f;

        public override int Steps => 19;

        public override string Unit => "s";

        public override string ToNodeString() => ToFixedDecimalNodeString(2);


        private static RtaDecayTime _minValue;

        private static RtaDecayTime _maxValue;

        public static RtaDecayTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

        public static RtaDecayTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

        protected RtaDecayTime() { }

        public RtaDecayTime(float unitValue) : base(unitValue) { }

        public RtaDecayTime(int stepValue) : base(stepValue) { }

        public static RtaDecayTime FromEncodedValue(float encodedValue) =>
            new RtaDecayTime() { EncodedValue = encodedValue };
    }
}
