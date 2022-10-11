using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct GateRange : ILinearFloat
{
    public static float MinUnitValue => 3f;
    public static float MaxUnitValue => 60f;
    public static float StepInterval => 1.0f;
    public static int Steps => ILinearFloat.StepsFromInterval(StepInterval, MinUnitValue, MaxUnitValue);
    public static string Unit => "dB";

    public static GateRange MinValue => new(IEncodedFloat.MinEncodedValue);
    public static GateRange MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private GateRange(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static GateRange FromEncodedValue(float encodedValue) => new(encodedValue);

    public static GateRange FromUnitValue(float unitValue) =>
        new(FloatConversions.LinearToEncoded(unitValue, MinUnitValue, MaxUnitValue));
    
    public static GateRange FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}