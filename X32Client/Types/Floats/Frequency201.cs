namespace Suhock.X32.Types.Floats;

public sealed class Frequency201 : Frequency
{
    public override float MinUnitValue => 20f;

    public override float MaxUnitValue => 20000f;

    public override int Steps => 201;


    private static Frequency201 _minValue;

    private static Frequency201 _maxValue;

    public static Frequency201 MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static Frequency201 MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    protected Frequency201(): base() { }

    public Frequency201(float unitValue) : base(unitValue) { }

    public Frequency201(int stepValue) : base(stepValue) { }

    public static Frequency201 FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };
}