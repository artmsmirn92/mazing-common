using UnityEngine.Events;

namespace mazing.common.Runtime
{
    public interface IInit
    {
        bool              Initialized { get; }
        event UnityAction Initialize;
        void              Init();
    }
}