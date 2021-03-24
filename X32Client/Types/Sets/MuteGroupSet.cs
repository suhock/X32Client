using System.Collections.Generic;

namespace Suhock.X32.Types.Sets
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming",
        "CA1710:Identifiers should have correct suffix",
        Justification = "<Pending>")]
    public class MuteGroupSet : BitSet
    {
        public MuteGroupSet() : base(6) { }

        public MuteGroupSet(int bits) : base(6, bits) { }

        public MuteGroupSet(ICollection<int> items) : base(6, items) { }
    }
}
