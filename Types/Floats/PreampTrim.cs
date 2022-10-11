using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct PreampTrim : ILinearFloat
{
    public static float MinUnitValue => -18f;
    public static float MaxUnitValue => 18f;
    public static float StepInterval => 0.25f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static PreampTrim MinValue => new(IEncodedFloat.MinEncodedValue);
    public static PreampTrim MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private PreampTrim(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static PreampTrim FromEncodedValue(float encodedValue) => new(encodedValue);

    public static PreampTrim FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static PreampTrim FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}