using Suhock.X32.Util;
using System;

namespace Suhock.X32.Types.Floats;

public abstract class AbstractLinearDecimal : AbstractSteppedDecimal
{
    protected AbstractLinearDecimal()
    {
    }

    public AbstractLinearDecimal(float unitValue) : base(unitValue)
    {
    }

    public AbstractLinearDecimal(int stepValue) : base(stepValue)
    {
    }

    public override float UnitValue
    {
        get => FloatConversions.EncodedToLinear(EncodedValue, MinUnitValue, MaxUnitValue);

        protected init => EncodedValue = FloatConversions.LinearToEncoded(value, MinUnitValue, MaxUnitValue);
    }

    public override int Steps => (int)Math.Round(1 / StepInterval) + 1;

    public abstract float StepInterval { get; }
}

