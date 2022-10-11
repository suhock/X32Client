namespace Suhock.X32.Types;

public enum X32Input
{
    Ch01, Ch02, Ch03, Ch04, Ch05, Ch06, Ch07, Ch08,
    Ch09, Ch10, Ch11, Ch12, Ch13, Ch14, Ch15, Ch16,
    Ch17, Ch18, Ch19, Ch20, Ch21, Ch22, Ch23, Ch24,
    Ch25, Ch26, Ch27, Ch28, Ch29, Ch30, Ch31, Ch32,
    Aux01, Aux02, Aux03, Aux04, Aux05, Aux06, Aux07, Aux08,
    FxRtn1L, FxRtn1R, FxRtn2L, FxRtn2R, FxRtn3L, FxRtn3R, FxRtn4L, FxRtn4R,
    MixBus01, MixBus02, MixBus03, MixBus04, MixBus05, MixBus06, MixBus07, MixBus08,
    MixBus09, MixBus10, MixBus11, MixBus12, MixBus13, MixBus14, MixBus15, MixBus16,
    Matrix1, Matrix2, Matrix3, Matrix4, Matrix5, Matrix6,
    MainLR, MainMC,
    DCA1, DCA2, DCA3, DCA4, DCA5, DCA6, DCA7, DCA8
}

public static class X32InputExtensions
{
    public static bool IsChannel(this X32Input input)
    {
        return input >= X32Input.Ch01 && input <= X32Input.Ch32;
    }

    public static bool IsAux(this X32Input input)
    {
        return input >= X32Input.Aux01 && input <= X32Input.Aux08;
    }
}