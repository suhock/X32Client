using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public abstract class AbstractLogDecimal : AbstractSteppedDecimal
{
    protected AbstractLogDecimal() { }

    protected AbstractLogDecimal(float unitValue) : base(unitValue) { }

    protected AbstractLogDecimal(int stepValue) : base(stepValue) { }

    public override float UnitValue
    {
        get => FloatConversions.EncodedToLog(EncodedValue, MinUnitValue, MaxUnitValue);

        protected init => EncodedValue = FloatConversions.LogToEncoded(value, MinUnitValue, MaxUnitValue);
    }
}