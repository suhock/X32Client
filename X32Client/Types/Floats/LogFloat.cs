using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats
{
    public abstract class LogFloat : DiscreteFloat
    {
        protected LogFloat() { }

        protected LogFloat(float unitValue) : base(unitValue) { }

        protected LogFloat(int stepValue) : base(stepValue) { }

        public override float UnitValue
        {
            get => FloatConversions.EncodedToLog(EncodedValue, MinUnitValue, MaxUnitValue);

            protected set => EncodedValue = FloatConversions.LogToEncoded(value, MinUnitValue, MaxUnitValue);
        }
    }
}
