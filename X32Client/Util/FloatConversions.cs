using System;

namespace Suhock.X32.Util
{
    class FloatConversions
    {
        public static float EncodedToLevel(float value)
        {
            if (value >= 0.5f)
            {
                return 40.0f * Math.Min(value, 1.0f) - 30.0f;
            }
            else if (value >= 0.25f)
            {
                return 80.0f * value - 50.0f;
            }
            else if (value >= 0.0625f)
            {
                return 160.0f * value - 70.0f;
            }
            else if (value > 0.0f)
            {
                return 480.0f * value - 90.0f;
            }
            else
            {
                return float.NegativeInfinity;
            }
        }

        public static float LevelToEncoded(float value)
        {
            if (value >= -10.0f)
            {
                return (Math.Min(value, 10.0f) + 30.0f) / 40.0f;
            }
            else if (value >= -30.0f)
            {
                return (value + 50.0f) / 80.0f;
            }
            else if (value >= -60.0f)
            {
                return (value + 70.0f) / 160.0f;
            }
            else if (value > -90.0f)
            {
                return (value + 90.0f) / 480.0f;
            }
            else
            {
                return 0.0f;
            }
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
}
