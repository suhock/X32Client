using System;

namespace Suhock.X32.Nodes
{

    public abstract class IndexedSlotClient<TConfig> : SlotClient<TConfig> where TConfig: SlotConfigClient
    {
        internal IndexedSlotClient(RootClient outer, string path, int maxId, int id) :
            base(outer, path + '/' + id.ToString().PadLeft(2, '0') + "/")
        {
            if (id < 1 || id > maxId)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "Must be between 1 and " + maxId);
            }

            Id = id;
        }

        public int Id { get; }
    }
}
