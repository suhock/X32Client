using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public interface ILevelFloat : IEncodedFloat
{
}

public static class LevelFloatExtensions
{
    public static float GetUnitValue<T>(this T value) where T: ILevelFloat
    {
        return FloatConversions.LevelToEncoded(value.EncodedValue);
    }

    public static string ToUnitString<T>(this T value) where T : ILevelFloat
    {
        return $"{value.GetUnitValue()}{T.Unit}";
    }
}