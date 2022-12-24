using UnityEngine;

namespace mazing.common.Runtime
{
    public static class CommonData
    {
        public static int  GameId = 1;
        public static bool DevelopmentBuild;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ResetState()
        {

            DevelopmentBuild = false;
#if !UNITY_EDITOR && DEVELOPMENT_BUILD
            DevelopmentBuild = true;
#endif
        }
    }
}