using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct ReleaseTime : ILogFloat
{
    public static float MinUnitValue => 5.0f;
    public static float MaxUnitValue => 4000f;
    public static int Steps => 101;
    public static string Unit => "ms";
    
    public static ReleaseTime MinValue => new(IEncodedFloat.MinEncodedValue);
    public static ReleaseTime MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private ReleaseTime(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static ReleaseTime FromEncodedValue(float encodedValue) => new(encodedValue);

    public static ReleaseTime FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static ReleaseTime FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}