using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct FaderLevel : ILevelFloat
{
    public static float MinUnitValue => float.NegativeInfinity;
    public static float MaxUnitValue => 10.0f;
    public static int Steps => 161;
    public static string Unit => "dB";

    public static FaderLevel MinValue => new(IEncodedFloat.MinEncodedValue);
    public static FaderLevel MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private FaderLevel(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static FaderLevel FromEncodedValue(float encodedValue) => new(encodedValue);

    public static FaderLevel FromUnitValue(float unitValue) =>
        new(FloatConversions.LevelToEncoded(unitValue));

    public static FaderLevel FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}