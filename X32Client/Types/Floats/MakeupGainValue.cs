using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct MakeupGainValue : ILinearFloat
{
    public static float MinUnitValue => 0f;
    public static float MaxUnitValue => 24f;
    public static float StepInterval => 0.5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static MakeupGainValue MinValue => new(IEncodedFloat.MinEncodedValue);
    public static MakeupGainValue MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private MakeupGainValue(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static MakeupGainValue FromEncodedValue(float encodedValue) => new(encodedValue);

    public static MakeupGainValue FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static MakeupGainValue FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}