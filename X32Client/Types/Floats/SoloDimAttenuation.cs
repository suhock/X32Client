namespace Suhock.X32.Types.Floats;

public sealed class SoloDimAttenuation : AbstractLinearDecimal
{
    private static SoloDimAttenuation? _minValue;

    private static SoloDimAttenuation? _maxValue;

    public static SoloDimAttenuation MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static SoloDimAttenuation MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private SoloDimAttenuation()
    {
    }

    public SoloDimAttenuation(float unitValue) : base(unitValue)
    {
    }

    public SoloDimAttenuation(int stepValue) : base(stepValue)
    {
    }

    public static SoloDimAttenuation FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -40f;

    public override float MaxUnitValue => 0f;

    public override float StepInterval => 1.0f;

    public override string Unit => "dB";
}