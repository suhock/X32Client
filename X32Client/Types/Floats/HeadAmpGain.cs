namespace Suhock.X32.Types.Floats;

public sealed class HeadAmpGain : AbstractLinearDecimal
{
    private static HeadAmpGain? _minValue;

    private static HeadAmpGain? _maxValue;

    public static HeadAmpGain MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static HeadAmpGain MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private HeadAmpGain()
    {
    }

    public HeadAmpGain(float unitValue) : base(unitValue)
    {
    }

    public HeadAmpGain(int stepValue) : base(stepValue)
    {
    }

    public static HeadAmpGain FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -12f;

    public override float MaxUnitValue => 60f;

    public override float StepInterval => 0.5f;

    public override string Unit => "dB";
}