namespace Suhock.X32.Types.Enums;

public enum InsertSelection
{
    Off,
    Fx1L, Fx1R, Fx2L, Fx2R, Fx3L, Fx3R, Fx4L, Fx4R,
    Fx5L, Fx5R, Fx6L, Fx6R, Fx7L, Fx7R, Fx8L, Fx8R,
    Aux1, Aux2, Aux3, Aux4, Aux5, Aux6
}

public static class InsertSelectionExtensions
{
    public static string ToNodeString(this InsertSelection input)
    {
        if (input == InsertSelection.Off)
        {
            return "OFF";
        }
        else if (input <= InsertSelection.Fx8R)
        {
            return "FX" + ((input - InsertSelection.Fx1L) / 2 + 1) +
                   ((input - InsertSelection.Fx1L) % 2 == 0 ? 'L' : 'R');
        }
        else
        {
            return "Aux" + ((int)input).ToString();
        }
    }

    public static bool IsOff(this Source source)
    {
        return source == Source.Off;
    }

    public static bool IsAux(this InsertSelection source)
    {
        return source >= InsertSelection.Aux1 && source <= InsertSelection.Aux6;
    }

    public static bool IsFx(this InsertSelection source)
    {
        return source >= InsertSelection.Fx1L && source <= InsertSelection.Fx8R;
    }
}