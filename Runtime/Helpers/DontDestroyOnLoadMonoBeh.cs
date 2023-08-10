using UnityEngine;

namespace mazing.common.Runtime.Helpers
{
    public class DontDestroyOnLoadMonoBeh : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
