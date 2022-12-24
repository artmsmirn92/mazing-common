using UnityEngine;

namespace mazing.common.Runtime.Helpers
{
    public interface IContainersGetter
    {
        Transform GetContainer(string _Name);
    }
}