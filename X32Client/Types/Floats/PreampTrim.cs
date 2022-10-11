namespace Suhock.X32.Types.Floats;

public sealed class PreampTrim : AbstractLinearDecimal
{
    private static PreampTrim? _minValue;

    private static PreampTrim? _maxValue;

    public static PreampTrim MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static PreampTrim MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private PreampTrim()
    {
    }

    public PreampTrim(float unitValue) : base(unitValue)
    {
    }

    public PreampTrim(int stepValue) : base(stepValue)
    {
    }

    public static PreampTrim FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -18f;

    public override float MaxUnitValue => 18f;

    public override float StepInterval => 0.25f;

    public override string Unit => "dB";
}