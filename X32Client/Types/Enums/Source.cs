namespace Suhock.X32.Types.Enums
{
    public enum Source
    {
        Off,
        In01, In02, In03, In04, In05, In06, In07, In08,
        In09, In10, In11, In12, In13, In14, In15, In16,
        In17, In18, In19, In20, In21, In22, In23, In24,
        In25, In26, In27, In28, In29, In30, In31, In32,
        Aux1, Aux2, Aux3, Aux4, Aux5, Aux6, UsbL, UsbR,
        Fx1L, Fx1R, Fx2L, Fx2R, Fx3L, Fx3R, Fx4L, Fx4R,
        Bus01, Bus02, Bus03, Bus04, Bus05, Bus06, Bus07, Bus08,
        Bus09, Bus10, Bus11, Bus12, Bus13, Bus14, Bus15, Bus16
    }

    public static class SourceExtensions
    {
        public static bool IsOff(this Source source)
        {
            return source == Source.Off;
        }

        public static bool IsChannel(this Source source)
        {
            return source >= Source.In01 && source <= Source.In32;
        }

        public static bool IsAux(this Source source)
        {
            return source >= Source.Aux1 && source <= Source.UsbR;
        }

        public static bool IsFx(this Source source)
        {
            return source >= Source.Fx1L && source <= Source.Fx4R;
        }

        public static bool IsBus(this Source source)
        {
            return source >= Source.Bus01;
        }
    }
}
