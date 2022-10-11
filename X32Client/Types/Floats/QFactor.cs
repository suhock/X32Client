using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public readonly struct QFactor : ILogFloat
{
    public static float MinUnitValue => 10;
    public static float MaxUnitValue => 0.3f;
    public static int Steps => 72;
    public static string Unit => "";
    
    public static QFactor MinValue => new(IEncodedFloat.MinEncodedValue);
    public static QFactor MaxValue => new(IEncodedFloat.MaxEncodedValue);

    public float EncodedValue { get; }

    private QFactor(float encodedValue)
    {
        EncodedValue = encodedValue;
    }

    public static QFactor FromEncodedValue(float encodedValue) => new(encodedValue);

    public static QFactor FromUnitValue(float unitValue) =>
        new(FloatConversions.LogToEncoded(unitValue, MinUnitValue, MaxUnitValue));

    public static QFactor FromStepValue(int stepValue) =>
        new(FloatConversions.StepToEncoded(stepValue, Steps));

    public override string ToString() => this.ToUnitString();
}