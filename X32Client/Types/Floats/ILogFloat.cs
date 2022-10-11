using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public interface ILogFloat : IEncodedFloat
{
}

public static class LogFloatExtensions
{
    public static float GetUnitValue<T>(this T value) where T: ILogFloat
    {
        return FloatConversions.LogToEncoded(value.EncodedValue, T.MinUnitValue, T.MaxUnitValue);
    }

    public static string ToUnitString<T>(this T value) where T : ILogFloat
    {
        return $"{value.GetUnitValue()}{T.Unit}";
    }
}