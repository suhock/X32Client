using Suhock.X32.Types.Floats;

namespace Suhock.X32.Routing;

internal sealed class HeadAmpState
{
    public HeadAmpState(HeadAmpGain gain, bool phantom)
    {
        Gain = gain;
        Phantom = phantom;
    }

    public HeadAmpGain Gain { get; init; }
    public bool Phantom { get; init; }
}