using Suhock.X32.Types.Sets;
using Suhock.X32.Types.Floats;
using System;
using System.Collections.Generic;

namespace Suhock.X32.Nodes
{
    public abstract class NodeClient
    {
        internal X32Client Client { get; }

        internal string AddressPrefix { get; }

        private readonly Dictionary<Type, object> _containers = new Dictionary<Type, object>();

        internal NodeClient(X32Client client, string addressPrefix)
        {
            Client = client;
            AddressPrefix = addressPrefix;
        }

        protected T GetNode<T>(params object[] args) where T : NodeClient
        {
            Type type = typeof(T);

            if (!_containers.ContainsKey(type))
            {
                object[] cArgs = new object[args.Length + 1];
                cArgs[0] = this;
                Array.Copy(args, 0, cArgs, 1, args.Length);
                _containers[type] = Activator.CreateInstance(type, cArgs);
            }

            return (T)_containers[type];
        }

        protected T GetGroupNode<T>(int length, int index, params object[] args) where T : NodeClient
        {
            Type groupType = typeof(T[]);
            Type type = typeof(T);

            if (!_containers.ContainsKey(groupType))
            {
                _containers[groupType] = new T[length];
            }

            if (((T[])_containers[groupType])[index] == null)
            {
                object[] cArgs = new object[args.Length + 1];
                cArgs[0] = this;
                Array.Copy(args, 0, cArgs, 1, args.Length);
                ((T[])_containers[groupType])[index] = (T)Activator.CreateInstance(type, cArgs);
            }

            return ((T[])_containers[groupType])[index];
        }

        public void SetValue(string path, object value)
        {
            if (value is DiscreteFloat @float)
            {
                value = @float.EncodedValue;
            }
            else if (value is BitSet @set)
            {
                value = @set.Bits;
            }

            Client.Send(Client.MessageFactory.Create(AddressPrefix + path, value));
        }

        public T GetValue<T>(string path)
        {
            return Client.Query(Client.MessageFactory.Create(AddressPrefix + path)).GetArgumentValue<T>(0);
        }

        public bool GetBoolValue(string path)
        {
            return GetBoolValue(path);
        }
    }
}
