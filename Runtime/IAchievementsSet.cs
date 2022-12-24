using System.Collections.Generic;

namespace mazing.common.Runtime
{
    public interface IAchievementsSet
    {
        Dictionary<ushort, string> GetSet();
    }
}