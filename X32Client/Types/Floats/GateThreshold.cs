namespace Suhock.X32.Types.Floats;

public sealed class GateThreshold : AbstractLinearDecimal
{
    private static GateThreshold? _minValue;

    private static GateThreshold? _maxValue;

    public static GateThreshold MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static GateThreshold MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private GateThreshold()
    {
    }

    public GateThreshold(float unitValue) : base(unitValue)
    {
    }

    public GateThreshold(int stepValue) : base(stepValue)
    {
    }

    public static GateThreshold FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -80.0f;

    public override float MaxUnitValue => 0.0f;

    public override float StepInterval => 0.5f;

    public override string Unit => "dB";
}