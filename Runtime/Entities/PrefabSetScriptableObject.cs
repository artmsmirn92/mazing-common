using System;
using mazing.common.Runtime.Helpers.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace mazing.common.Runtime.Entities
{
    [CreateAssetMenu(fileName = "new_set", menuName = "Configs and Sets/Prefab Set", order = 1)]
    public class PrefabSetScriptableObject : ScriptableObject
    {
        [Serializable]
        public class Prefab
        {
            public string name;
            public Object item;
            public bool   bundle;
        }
    
        [Serializable]
        public class PrefabsList : Helpers.ReorderableArray<Prefab> { }

        [Header("Prefabs"), Reorderable(paginate = true, pageSize = 100)]
        public PrefabsList prefabs;
    }
}


