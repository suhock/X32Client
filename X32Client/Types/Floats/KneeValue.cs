namespace Suhock.X32.Types.Floats;

public sealed class KneeValue : AbstractLinearDecimal
{
    private static KneeValue? _minValue;

    private static KneeValue? _maxValue;

    public static KneeValue MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static KneeValue MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private KneeValue()
    {
    }

    public KneeValue(float unitValue) : base(unitValue)
    {
    }

    public KneeValue(int stepValue) : base(stepValue)
    {
    }

    public static KneeValue FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => 0f;

    public override float MaxUnitValue => 5f;

    public override float StepInterval => 1f;

    public override string Unit => "";
}