using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace mazing.common.Runtime.Helpers
{
    public class MazingCoroutinesRunner : MonoBehaviour
    {
        #region serialized fields

        [HideInInspector] public          bool isDestroyed;
        [HideInInspector] public volatile bool mustRun;

        #endregion

        #region api

        public readonly List<UnityAction> Actions = new();

        #endregion

        #region engine methods

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!mustRun)
                return;
            try
            {
                foreach (var action in Actions)
                    action?.Invoke();
            }
            catch (Exception e)
            {
                Dbg.LogError(e);
            }

            Actions.Clear();
            mustRun = false;
        }

        private void OnDestroy()
        {
            isDestroyed = true;
        }

        #endregion
    }
}