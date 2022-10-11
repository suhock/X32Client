using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct MixPercentage : ILinearFloat
{
    public static float MinUnitValue => 0f;
    public static float MaxUnitValue => 100f;
    public static float StepInterval => 5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "%";

    public static MixPercentage MinValue => new(IEncodedFloat.MinEncodedValue);
    public static MixPercentage MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private MixPercentage(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static MixPercentage FromEncodedValue(float encodedValue) => new(encodedValue);

    public static MixPercentage FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static MixPercentage FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}