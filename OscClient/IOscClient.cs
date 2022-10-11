using System.Threading;
using System.Threading.Tasks;

namespace Suhock.Osc;

public interface IOscClient
{
    OscMessage Receive();
    
    Task<OscMessage> ReceiveAsync() => ReceiveAsync(CancellationToken.None);
    
    Task<OscMessage> ReceiveAsync(CancellationToken cancellationToken);
    
    void Send(OscMessage msg);
    
    Task SendAsync(OscMessage msg) => SendAsync(msg, CancellationToken.None);
    
    Task SendAsync(OscMessage msg, CancellationToken cancellationToken);
}