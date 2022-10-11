namespace Suhock.X32.Types.Floats;

public sealed class ReleaseTime : AbstractLogDecimal
{
    private static ReleaseTime? _minValue;

    private static ReleaseTime? _maxValue;

    public static ReleaseTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static ReleaseTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private ReleaseTime() { }

    public ReleaseTime(float unitValue) : base(unitValue) { }

    public ReleaseTime(int stepValue) : base(stepValue) { }

    public static ReleaseTime FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => 5.0f;

    public override float MaxUnitValue => 4000f;

    public override int Steps => 101;

    public override string Unit => "ms";

    public override string ToNodeString() => ToFixedDecimalNodeString(0);
}