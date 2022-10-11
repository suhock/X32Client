namespace Suhock.X32.Types.Floats;

public sealed class Frequency121 : Frequency
{
    public override float MinUnitValue => 20f;

    public override float MaxUnitValue => 20000f;

    public override int Steps => 121;

    public override string Unit => "Hz";


    private static Frequency121 _minValue;

    private static Frequency121 _maxValue;

    public static Frequency121 MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static Frequency121 MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private Frequency121()
    {
    }

    public Frequency121(float unitValue) : base(unitValue)
    {
    }

    public Frequency121(int stepValue) : base(stepValue)
    {
    }

    public static Frequency121 FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };
}