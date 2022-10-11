using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct DelayTime : ILinearFloat
{
    public static float MinUnitValue => -0.3f;
    public static float MaxUnitValue => 500.0f;
    public static float StepInterval => 0.1f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "ms";

    public static DelayTime MinValue => new(IEncodedFloat.MinEncodedValue);
    public static DelayTime MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private DelayTime(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static DelayTime FromEncodedValue(float encodedValue) => new(encodedValue);

    public static DelayTime FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));
    
    public static DelayTime FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}