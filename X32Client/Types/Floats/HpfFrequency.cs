using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct HpfFrequency : ILogFloat
{
    public static float MinUnitValue => 20f;
    public static float MaxUnitValue => 400f;
    public static int Steps => 101;
    public static string Unit => "Hz";

    public static HpfFrequency MinValue => new(IEncodedFloat.MinEncodedValue);
    public static HpfFrequency MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private HpfFrequency(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static HpfFrequency FromEncodedValue(float encodedValue) => new(encodedValue);

    public static HpfFrequency FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static HpfFrequency FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}