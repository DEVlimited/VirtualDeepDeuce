using Arcspark.DataToolkit;
using UnityEngine;

namespace Arcspark.Sample.CacheUtilSample
{
    public class Sample : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            CacheUtil.Instance.SetValue("a", 10);
            bool success = CacheUtil.Instance.GetValue("a", out int a);
            if (success)
                Debug.Log(a);
        }
    }
}