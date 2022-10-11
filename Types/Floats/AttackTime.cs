using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct AttackTime : ILinearFloat
{
    public static float MinUnitValue => 0.0f;
    public static float MaxUnitValue => 120.0f;
    public static float StepInterval => 1.0f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "ms";

    public static AttackTime MinValue => new(IEncodedFloat.MinEncodedValue);
    public static AttackTime MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private AttackTime(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static AttackTime FromEncodedValue(float encodedValue) => new(encodedValue);

    public static AttackTime FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static AttackTime FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}