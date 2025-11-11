using System.Collections.Generic;
using Elsa.Client.Models;

namespace Elsa.Client.Comparers
{
    public class BlockingActivityEqualityComparer : IEqualityComparer<BlockingActivity>
    {
        public static BlockingActivityEqualityComparer Instance { get; } = new BlockingActivityEqualityComparer();

        public bool Equals(BlockingActivity? x, BlockingActivity? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.ActivityId.Equals(y.ActivityId);
        }

        public int GetHashCode(BlockingActivity obj)
        {
            return obj.ActivityId?.GetHashCode() ?? 0;
        }
    }
}