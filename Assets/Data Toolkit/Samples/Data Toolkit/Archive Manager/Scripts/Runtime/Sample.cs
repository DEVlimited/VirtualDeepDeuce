using Arcspark.DataToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcspark.Sample.ArchiveManagerSample
{
    [Serializable]
    public class MyArchiveData
    {
        public int A = 0;
        public List<int> B = new List<int>();
        public SerializeableDictionary<string, int> C = new SerializeableDictionary<string, int> ();
        public SerializeableDictionary<string, double> D = new SerializeableDictionary<string, double>();
    }

    public class MyArchiveManager : ArchiveManager<MyArchiveData>
    {

        public override void Load()
        {
            Debug.Log("On Load");
            base.Load();
        }

        public override void Save()
        {
            Debug.Log("On Save");
            base.Save();
        }

        static MyArchiveManager() { }

        public static MyArchiveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syslock)
                    {
                        if (instance == null)
                        {
                            instance = new MyArchiveManager();
                        }
                    }
                }
                return instance;
            }
        }

        private static MyArchiveManager instance;
        private static readonly object syslock = new object();
    }

    public class Sample : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            MyArchiveManager.Instance.Archive.A = 100;
            MyArchiveManager.Instance.Archive.B = new List<int>{2,22,222};
            MyArchiveManager.Instance.Archive.C.Dictionary.Add("key0", 0);
            MyArchiveManager.Instance.Archive.C.Dictionary.Add("key1", 1);
            MyArchiveManager.Instance.Archive.D.Dictionary.Add("d0", 1.2);
            MyArchiveManager.Instance.Archive.D.Dictionary.Add("d1", 1.3);

            MyArchiveManager.Instance.Save();
            MyArchiveManager.Instance.Clear();
            MyArchiveManager.Instance.Load();
            
            Debug.Log(MyArchiveManager.Instance.Archive.A);
            Debug.Log(MyArchiveManager.Instance.Archive.B);
            Debug.Log(MyArchiveManager.Instance.Archive.C["key0"]);
            Debug.Log(MyArchiveManager.Instance.Archive.C["key1"]);
            Debug.Log(MyArchiveManager.Instance.Archive.D["d0"]);
            Debug.Log(MyArchiveManager.Instance.Archive.D["d1"]);
        }
    }
}