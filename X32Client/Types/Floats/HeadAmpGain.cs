using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct HeadAmpGain : ILinearFloat
{
    public static float MinUnitValue => -12f;
    public static float MaxUnitValue => 60f;
    public static float StepInterval => 0.5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static HeadAmpGain MinValue => new(IEncodedFloat.MinEncodedValue);
    public static HeadAmpGain MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private HeadAmpGain(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static HeadAmpGain FromEncodedValue(float encodedValue) => new(encodedValue);

    public static HeadAmpGain FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static HeadAmpGain FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}