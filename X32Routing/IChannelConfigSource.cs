using System.Collections.Generic;
using System.Threading.Tasks;
using Suhock.X32.Routing.Config;

namespace Suhock.X32.Routing;

internal interface IChannelConfigSource
{
    public Task<IEnumerable<ChannelConfig>> GetChannelConfigAsync();
}