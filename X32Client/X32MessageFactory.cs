using Suhock.Osc;
using Suhock.X32.Types.Enums;

namespace Suhock.X32
{
    public class X32MessageFactory : OscMessageFactory
    {
        public X32MessageFactory() : base(new X32ArgumentFactory()) { }

        public static string ChannelAddress(int channel, string path)
        {
            return "/ch/" + channel.ToString().PadLeft(2, '0') + path;
        }

        public OscMessage Channel(int channel, string path, params object[] args)
        {
            return Create(ChannelAddress(channel, path) + path, args);
        }

        public OscMessage GetChannelColor(int channel)
        {
            return Channel(channel, "/config/color");
        }

        public OscMessage SetChannelColor(int channel, StripColor color)
        {
            return Channel(channel, "/config/color", color);
        }

        public OscMessage GetChannelMixOn(int channel)
        {
            return Channel(channel, "/mix/on");
        }

        public OscMessage SetChannelMixOn(int channel, bool on)
        {
            return Channel(channel, "/mix/on", on);
        }
    }
}
