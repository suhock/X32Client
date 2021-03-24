using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats
{
    public abstract class LevelFloat : DiscreteFloat
    {
        protected LevelFloat() { }

        public LevelFloat(float unitValue) : base(unitValue) { }

        public LevelFloat(int stepValue) : base(stepValue) { }

        public override float UnitValue
        {
            get => FloatConversions.EncodedToLevel(EncodedValue);

            protected set => EncodedValue = FloatConversions.LevelToEncoded(value);
        }

        public override float MinUnitValue => float.NegativeInfinity;

        public override float MaxUnitValue => 10.0f;

        public override string Unit => "dB";
    }
}
