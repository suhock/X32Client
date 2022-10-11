using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct HoldTime : ILogFloat
{
    public static float MinUnitValue => 0.02f;
    public static float MaxUnitValue => 2000.0f;
    public static int Steps => 101;
    public static string Unit => "ms";
    
    public static HoldTime MinValue => new(IEncodedFloat.MinEncodedValue);
    public static HoldTime MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private HoldTime(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static HoldTime FromEncodedValue(float encodedValue) => new(encodedValue);

    public static HoldTime FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static HoldTime FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}