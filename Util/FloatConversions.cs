using System;

namespace Suhock.X32.Util;

internal static class FloatConversions
{
    public static int EncodedToStep(float encodedValue, int stepCount)
    {
        return (int)Math.Round(encodedValue * (stepCount - 1));
    }

    public static float StepToEncoded(int step, int stepCount)
    {
        return (float)step / (stepCount - 1);
    }

    public static float EncodedToLevel(float value)
    {
        return value switch
        {
            >= 0.5f => 40.0f * Math.Min(value, 1.0f) - 30.0f,
            >= 0.25f => 80.0f * value - 50.0f,
            >= 0.0625f => 160.0f * value - 70.0f,
            > 0.0f => 480.0f * value - 90.0f,
            _ => float.NegativeInfinity
        };
    }

    public static float LevelToEncoded(float value)
    {
        return value switch
        {
            >= -10.0f => (Math.Min(value, 10.0f) + 30.0f) / 40.0f,
            >= -30.0f => (value + 50.0f) / 80.0f,
            >= -60.0f => (value + 70.0f) / 160.0f,
            > -90.0f => (value + 90.0f) / 480.0f,
            _ => 0.0f
        };
    }

    public static float EncodedToLinear(float value, float rangeMin, float rangeMax)
    {
        return value * (rangeMax - rangeMin) + rangeMin;
    }

    public static float LinearToEncoded(float value, float rangeMin, float rangeMax)
    {
        return (value - rangeMin) / (rangeMax - rangeMin);
    }
    
    public static float EncodedToLog(float value, float minValue, float maxValue)
    {
        return minValue * (float)Math.Pow(maxValue / minValue, value);
    }

    public static float LogToEncoded(float value, float minValue, float maxValue)
    {
        return (float)(Math.Log(value / minValue) / Math.Log(maxValue / minValue));
    }

    public static float AlignEncoded(float value, int maxInterval)
    {
        return Math.Clamp((float)Math.Round(value * maxInterval) / maxInterval, 0.0f, 1.0f);
    }
}