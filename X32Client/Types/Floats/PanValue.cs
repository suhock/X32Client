using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct PanValue : ILinearFloat
{
    public static float MinUnitValue => -100f;
    public static float MaxUnitValue => 100f;
    public static float StepInterval => 2f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "%";

    public static PanValue MinValue => new(IEncodedFloat.MinEncodedValue);
    public static PanValue MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private PanValue(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static PanValue FromEncodedValue(float encodedValue) => new(encodedValue);

    public static PanValue FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static PanValue FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}