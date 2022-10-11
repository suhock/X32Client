using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct SoloDimAttenuation : ILinearFloat
{
    public static float MinUnitValue => -40f;
    public static float MaxUnitValue => 0f;
    public static float StepInterval => 1.0f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static SoloDimAttenuation MinValue => new(IEncodedFloat.MinEncodedValue);
    public static SoloDimAttenuation MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private SoloDimAttenuation(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static SoloDimAttenuation FromEncodedValue(float encodedValue) => new(encodedValue);

    public static SoloDimAttenuation FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static SoloDimAttenuation FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}