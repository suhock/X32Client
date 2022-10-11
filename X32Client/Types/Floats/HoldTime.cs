namespace Suhock.X32.Types.Floats;

public sealed class HoldTime : AbstractLogDecimal
{
    private static HoldTime? _minValue;

    private static HoldTime? _maxValue;

    public static HoldTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static HoldTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private HoldTime()
    {
    }

    public HoldTime(float unitValue) : base(unitValue)
    {
    }

    public HoldTime(int stepValue) : base(stepValue)
    {
    }

    public static HoldTime FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => 0.02f;

    public override float MaxUnitValue => 2000.0f;

    public override int Steps => 101;

    public override string Unit => "ms";


    public override string ToNodeString()
    {
        return UnitValue switch
        {
            < 10.0f => ToFixedDecimalNodeString(2),
            < 100.0f => ToFixedDecimalNodeString(1),
            _ => ToFixedDecimalNodeString(0)
        };
    }
}