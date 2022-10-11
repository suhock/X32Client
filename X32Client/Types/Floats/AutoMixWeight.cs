namespace Suhock.X32.Types.Floats;

public sealed class AutoMixWeight : AbstractLinearDecimal
{
    private static AutoMixWeight? _minValue;

    private static AutoMixWeight? _maxValue;

    public static AutoMixWeight MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static AutoMixWeight MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private AutoMixWeight()
    {
    }

    public AutoMixWeight(float unitValue) : base(unitValue)
    {
    }

    public AutoMixWeight(int stepValue) : base(stepValue)
    {
    }

    public static AutoMixWeight FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -12f;

    public override float MaxUnitValue => 12f;

    public override float StepInterval => 0.5f;

    public override string Unit => "";
}