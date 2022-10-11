namespace Suhock.X32.Types.Floats;

public sealed class MakeupGainValue : AbstractLinearDecimal
{
    private static MakeupGainValue? _minValue;

    private static MakeupGainValue? _maxValue;

    public static MakeupGainValue MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static MakeupGainValue MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private MakeupGainValue()
    {
    }

    public MakeupGainValue(float unitValue) : base(unitValue)
    {
    }

    public MakeupGainValue(int stepValue) : base(stepValue)
    {
    }

    public static MakeupGainValue FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => 0f;

    public override float MaxUnitValue => 24f;

    public override float StepInterval => 0.5f;

    public override string Unit => "dB";
}