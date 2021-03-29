using System;
using System.Threading.Tasks;

namespace Suhock.Osc
{
    public class OscClient
    {
        /// <summary>
        /// Creates an OSC client instance with a default OscMessageFactory implementation, accepting the default
        /// argument types specified in the OSC specification.
        /// </summary>
        /// <param name="connection">an OSC connection implementation</param>
        public OscClient(IOscConnection connection)
        {
            Connection = connection;
            MessageFactory = new OscMessageFactory(new OscArgumentFactory());
        }

        /// <summary>
        /// Creates an OSC client instance.
        /// </summary>
        /// <param name="connection">an OSC connection implementation</param>
        /// <param name="messageFactory">a </param>
        public OscClient(IOscConnection connection, IOscMessageFactory messageFactory)
        {
            Connection = connection;
            MessageFactory = messageFactory;
        }

        public IOscConnection Connection { get; }

        public IOscMessageFactory MessageFactory { get; }

        private void VerifyState()
        {
        }

        public OscMessage Receive()
        {
            VerifyState();

            return MessageFactory.Create(Connection.Receive(), out _);
        }

        public async Task<OscMessage> ReceiveAsync()
        {
            VerifyState();

            return MessageFactory.Create(await Connection.ReceiveAsync(), out _);
        }

        public void Send(OscMessage msg)
        {
            VerifyState();

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            Connection.Send(msg.GetPacketBytes());
        }

        public async Task SendAsync(OscMessage msg)
        {
            VerifyState();

            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            await Connection.SendAsync(msg.GetPacketBytes());
        }
    }
}
