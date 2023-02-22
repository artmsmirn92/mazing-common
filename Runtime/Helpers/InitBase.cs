using UnityEngine.Events;

namespace mazing.common.Runtime.Helpers
{
    public abstract class InitBase : IInit
    {
        public bool              Initialized { get; private set; }
        public event UnityAction Initialize;
        
        public virtual void Init()
        {
            if (Initialized)
                return;
            RaiseInitialization();
        }

        protected void RaiseInitialization()
        {
            Initialized = true;
            Initialize?.Invoke();
        }
    }
}