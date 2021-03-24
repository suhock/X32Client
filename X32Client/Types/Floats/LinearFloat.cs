using Suhock.X32.Util;
using System;

namespace Suhock.X32.Types.Floats
{
    public abstract class LinearFloat : DiscreteFloat
    {
        protected LinearFloat() { }

        public LinearFloat(float unitValue) : base(unitValue) { }

        public LinearFloat(int stepValue) : base(stepValue) { }

        public override float UnitValue
        {
            get => FloatConversions.EncodedToLinear(EncodedValue, MinUnitValue, MaxUnitValue);

            protected set => EncodedValue = FloatConversions.LinearToEncoded(value, MinUnitValue, MaxUnitValue);
        }

        public override int Steps => (int)Math.Round(1 / StepInterval) + 1;

        public abstract float StepInterval { get; }
    }
}
