using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct RtaDecayTime : ILogFloat
{
    public static float MinUnitValue => 0.25f;
    public static float MaxUnitValue => 16.0f;
    public static int Steps => 19;
    public static string Unit => "s";

    public static RtaDecayTime MinValue => new(IEncodedFloat.MinEncodedValue);
    public static RtaDecayTime MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private RtaDecayTime(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static RtaDecayTime FromEncodedValue(float encodedValue) => new(encodedValue);

    public static RtaDecayTime FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static RtaDecayTime FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}