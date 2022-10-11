namespace Suhock.X32.Types.Floats;

public sealed class FaderLevel : AbstractLevelDecimal
{
    public override int Steps => 161;

    public override string ToNodeString() => (UnitValue >= 0 ? "+" : "") + ToFixedDecimalNodeString(1);


    private static FaderLevel? _minValue;

    private static FaderLevel? _maxValue;

    public static FaderLevel MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static FaderLevel MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private FaderLevel()
    {
    }

    public FaderLevel(float unitValue) : base(unitValue)
    {
    }

    public FaderLevel(int stepValue) : base(stepValue)
    {
    }

    public static FaderLevel FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };
}