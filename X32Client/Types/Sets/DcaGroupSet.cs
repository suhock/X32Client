using System.Collections.Generic;

namespace Suhock.X32.Types.Sets
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming",
        "CA1710:Identifiers should have correct suffix",
        Justification = "<Pending>")]
    public class DcaGroupSet : BitSet
    {
        public DcaGroupSet() : base(8) { }

        public DcaGroupSet(int bits) : base(8, bits) { }

        public DcaGroupSet(ICollection<int> items) : base(8, items) { }
    }
}
