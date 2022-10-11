using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct DynamicsThreshold : ILinearFloat
{
    public static float MinUnitValue => -60.0f;
    public static float MaxUnitValue => 0.0f;
    public static float StepInterval => 0.5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static DynamicsThreshold MinValue => new(IEncodedFloat.MinEncodedValue);
    public static DynamicsThreshold MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private DynamicsThreshold(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static DynamicsThreshold FromEncodedValue(float encodedValue) => new(encodedValue);

    public static DynamicsThreshold FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));
    
    public static DynamicsThreshold FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}