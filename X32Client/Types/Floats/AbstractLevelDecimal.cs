using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public abstract class AbstractLevelDecimal : AbstractSteppedDecimal
{
    protected AbstractLevelDecimal()
    {
    }

    public AbstractLevelDecimal(float unitValue) : base(unitValue)
    {
    }

    public AbstractLevelDecimal(int stepValue) : base(stepValue)
    {
    }

    public override float UnitValue
    {
        get => FloatConversions.EncodedToLevel(EncodedValue);

        protected init => EncodedValue = FloatConversions.LevelToEncoded(value);
    }

    public override float MinUnitValue => float.NegativeInfinity;

    public override float MaxUnitValue => 10.0f;

    public override string Unit => "dB";
}