using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public sealed class Frequency121 : ILogFloat
{
    public static float MinUnitValue => 20f;
    public static float MaxUnitValue => 20000f;
    public static int Steps => 121;
    public static string Unit => "Hz";

    public static Frequency121 MinValue => new(IEncodedFloat.MinEncodedValue);
    public static Frequency121 MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private Frequency121(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static Frequency121 FromEncodedValue(float encodedValue) => new(encodedValue);

    public static Frequency121 FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static Frequency121 FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}