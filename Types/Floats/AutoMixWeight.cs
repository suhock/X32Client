using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct AutoMixWeight : ILinearFloat
{
    public static float MinUnitValue => -12f;
    public static float MaxUnitValue => 12f;
    public static float StepInterval => 0.5f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "";

    public static AutoMixWeight MinValue => FromEncodedValue(IEncodedFloat.MinEncodedValue);
    public static AutoMixWeight MaxValue => FromEncodedValue(IEncodedFloat.MaxEncodedValue);


    public float EncodedValue { get; }


    private AutoMixWeight(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static AutoMixWeight FromEncodedValue(float encodedValue) => new AutoMixWeight(encodedValue);

    public static AutoMixWeight FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static AutoMixWeight FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}