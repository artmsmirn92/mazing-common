using System.Collections.Generic;

namespace mazing.common.Runtime
{
    public interface ILeaderboardsSet
    {
        Dictionary<ushort, string> GetSet();
    }
}