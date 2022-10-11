using Suhock.X32.Util;

namespace Suhock.X32.Types.Floats;

public interface ILinearFloat : IEncodedFloat
{
    static abstract float StepInterval { get; }
}

public static class LinearFloatExtensions
{
    public static float GetUnitValue<T>(this T value) where T: ILinearFloat
    {
        return FloatConversions.LinearToEncoded(value.EncodedValue, T.MinUnitValue, T.MaxUnitValue);
    }

    public static string ToUnitString<T>(this T value) where T : ILinearFloat
    {
        return $"{value.GetUnitValue()}{T.Unit}";
    }
}