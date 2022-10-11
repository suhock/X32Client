namespace Suhock.X32.Types.Floats;

public sealed class MixPercentage : AbstractLinearDecimal
{
    private static MixPercentage? _minValue;

    private static MixPercentage? _maxValue;

    public static MixPercentage MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static MixPercentage MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private MixPercentage()
    {
    }

    public MixPercentage(float unitValue) : base(unitValue)
    {
    }

    public MixPercentage(int stepValue) : base(stepValue)
    {
    }

    public static MixPercentage FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => 0f;

    public override float MaxUnitValue => 100f;

    public override float StepInterval => 5f;

    public override string Unit => "%";
}