namespace Suhock.X32.Types.Floats;

public sealed class DelayTime : AbstractLinearDecimal
{
    private static DelayTime? _minValue;

    private static DelayTime? _maxValue;

    public static DelayTime MinValue => _minValue ??= FromEncodedValue(MinEncodedValue);

    public static DelayTime MaxValue => _maxValue ??= FromEncodedValue(MaxEncodedValue);

    private DelayTime()
    {
    }

    public DelayTime(float unitValue) : base(unitValue)
    {
    }

    public DelayTime(int stepValue) : base(stepValue)
    {
    }

    public static DelayTime FromEncodedValue(float encodedValue) => new() { EncodedValue = encodedValue };

    public override float MinUnitValue => -0.3f;

    public override float MaxUnitValue => 500.0f;

    public override float StepInterval => 0.1f;

    public override string Unit => "ms";
}