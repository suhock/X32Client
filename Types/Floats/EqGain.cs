using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct EqGain : ILinearFloat
{
    public static float MinUnitValue => -15f;
    public static float MaxUnitValue => 15f;
    public static float StepInterval => 0.25f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static EqGain MinValue => new(IEncodedFloat.MinEncodedValue);
    public static EqGain MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private EqGain(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static EqGain FromEncodedValue(float encodedValue) => new(encodedValue);

    public static EqGain FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));
    
    public static EqGain FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}