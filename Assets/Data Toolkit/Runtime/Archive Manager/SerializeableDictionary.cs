using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// Ability to provide Unity JSON serialization for Dictionary.
    /// </summary>
    [Serializable]
    public class SerializeableDictionary<K, V> : ISerializationCallbackReceiver
    {
        public V this[K key]
        {
            get
            {
                if (Dictionary.ContainsKey(key))
                    return Dictionary[key];
                return default(V);
            }

            set
            {
                Dictionary[key] = value;
            }
        }

        public void OnAfterDeserialize()
        {
            int len = keys.Count;
            Dictionary = new Dictionary<K, V>();
            for (int i = 0; i < len; ++i)
            {
                Dictionary[keys[i]] = values[i];
            }
            keys = null;
            values = null;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<K>();
            values = new List<V>();

            foreach (var kv in Dictionary)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }

        [SerializeField]
        private List<K> keys = new List<K>();
        [SerializeField]
        private List<V> values = new List<V>();
        [NonSerialized]
        public Dictionary<K, V> Dictionary = new Dictionary<K, V>();
    }
}