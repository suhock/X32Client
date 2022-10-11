using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct KneeValue : ILinearFloat
{
    public static float MinUnitValue => 0f;
    public static float MaxUnitValue => 5f;
    public static float StepInterval => 1f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "";

    public static KneeValue MinValue => new(IEncodedFloat.MinEncodedValue);
    public static KneeValue MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private KneeValue(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static KneeValue FromEncodedValue(float encodedValue) => new(encodedValue);

    public static KneeValue FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static KneeValue FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}