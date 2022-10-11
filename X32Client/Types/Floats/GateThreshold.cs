using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct GateThreshold : ILinearFloat
{
    public static float MinUnitValue => -80.0f;
    public static float MaxUnitValue => 0.0f;
    public static float StepInterval => 0.5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static GateThreshold MinValue => new(IEncodedFloat.MinEncodedValue);
    public static GateThreshold MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private GateThreshold(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static GateThreshold FromEncodedValue(float encodedValue) => new(encodedValue);

    public static GateThreshold FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static GateThreshold FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}