using Arcspark.DataToolkit;
using UnityEngine;

namespace Arcspark.Sample.ArchiveManagerSample
{
    public class Sample1 : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            SerializeableDictionary<string, int> d = new SerializeableDictionary<string, int>();
            d["0"] = 0;
            d["1"] = 1;

            string json = JsonUtility.ToJson(d);
            SerializeableDictionary<string, int> d1 = JsonUtility.FromJson<SerializeableDictionary<string, int>>(json);

            foreach (var kv in d1.Dictionary)
            {
                Debug.Log(string.Format("Key: {0}, Value: {1}", kv.Key, kv.Value));   
            }
        }
    }
}