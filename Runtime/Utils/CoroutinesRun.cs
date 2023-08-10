using System.Collections;
using System.Collections.Generic;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace mazing.common.Runtime.Utils
{
    public static partial class Cor
    {
        #region nonpublic members
        
        private static          MazingCoroutinesRunner _mazingCoroutineRunner;
        private static readonly List<IEnumerator>       RunningCoroutines = new List<IEnumerator>();

        #endregion

        #region api
        
        public static int GetRunningCoroutinesCount()
        {
            return RunningCoroutines.Count;
        }
        
        public static void Run(IEnumerator _Coroutine)
        {
            if (_Coroutine == null)
                return;
            if (_mazingCoroutineRunner.isDestroyed)
                return;
            var facade = FacadeCoroutine(_Coroutine);
            _mazingCoroutineRunner.StartCoroutine(facade);
        }

        public static void Stop(IEnumerator _Coroutine)
        {
            if (_Coroutine == null)
                return;
            if (_mazingCoroutineRunner.isDestroyed)
                return;
            RunningCoroutines.Remove(_Coroutine);
            _mazingCoroutineRunner.StopCoroutine(_Coroutine);
        }

        public static void RunSync(UnityAction _Action)
        {
            _mazingCoroutineRunner.Actions.Add(_Action);
            _mazingCoroutineRunner.mustRun = true;
        }

        public static bool IsRunning(IEnumerator _Coroutine)
        {
            return RunningCoroutines.Contains(_Coroutine);
        }

        #endregion

        #region nonpublic methods
        
        private static IEnumerator FacadeCoroutine(IEnumerator _Coroutine)
        {
            RunningCoroutines.Add(_Coroutine);
            yield return _Coroutine;
            RunningCoroutines.Remove(_Coroutine);
        }

        #endregion

        #region engine methods
        
        [RuntimeInitializeOnLoadMethod]
        private static void ResetState()
        {
#if UNITY_EDITOR
            RunningCoroutines.Clear();
#endif
            _mazingCoroutineRunner = Object.FindObjectOfType<MazingCoroutinesRunner>();
            if (_mazingCoroutineRunner.IsNotNull())
                return;
            var go = new GameObject("MAZING COROUTINES RUNNER");
            _mazingCoroutineRunner = go.AddComponent<MazingCoroutinesRunner>();
        }
        
        #endregion
    }
}
