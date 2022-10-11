using System;

namespace Suhock.X32.Types.Floats;

public interface IEncodedFloat
{
    public const float MinEncodedValue = 0.0f;
    public const float MaxEncodedValue = 1.0f;

    static abstract float MinUnitValue { get; }
    static abstract float MaxUnitValue { get; }
    static abstract string Unit { get; }
    
    static abstract int Steps { get; }
    
    float EncodedValue { get; }

    protected static int StepsFromInterval(float interval, float min, float max)
    {
        return (int)Math.Round((max - min) / interval) + 1;
    }
}

public static class EncodedFloatExtensions
{
    public static float MaxStepValue<T>(this T value) where T : IEncodedFloat
    {
        return T.Steps - 1;
    }
    
    public static float GetStepValue<T>(this T value) where T : IEncodedFloat
    {
        return (int)Math.Round(value.EncodedValue * MaxStepValue(value));
    }
}