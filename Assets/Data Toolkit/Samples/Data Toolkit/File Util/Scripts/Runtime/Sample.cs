using Arcspark.DataToolkit;
using UnityEngine;

namespace Arcspark.Sample.FileUtilSample
{
    public class Sample : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            string ft = "Samples/Data Toolkit/testfile.txt";

            string fileText = FileUtil.Instance.GetLocalFileText(
                FileUtil.LocalSourceType.STREAMING_ASSETS,
                ft
            );

            Debug.Log(string.Format("File text is: \"{0}\"", fileText));
        }
    }
}