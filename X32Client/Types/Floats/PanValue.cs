namespace Suhock.X32.Types.Floats;

public sealed class PanValue : AbstractLinearDecimal
{
    private static PanValue? _minValue;

    private static PanValue? _maxValue;

    public static PanValue MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static PanValue MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private PanValue()
    {
    }

    public PanValue(float unitValue) : base(unitValue)
    {
    }

    public PanValue(int stepValue) : base(stepValue)
    {
    }

    public static PanValue FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -100f;

    public override float MaxUnitValue => 100f;

    public override float StepInterval => 2f;

    public override string Unit => "%";
}