using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// A local archive manager.
    /// It can serialize the data into JSON and then access it in local file.
    /// </summary>
    /// <typeparam name="ArchiveDataType">
    /// The data type that needs to be serialized into a JSON Archive.
    /// ArchiveDataType MUST have token [Serializable].
    /// Members of ArchiveDataType MUST be public or have a [SerializeField] attribute.
    /// ArchiveDataType CAN have member of type SerializeableDictionary or any type that inherits the ISerializationCallbackReceiver.
    /// </typeparam>
    public class ArchiveManager<ArchiveDataType>
        where ArchiveDataType : class, new()
    {
        /// <summary>
        /// Load data from archive file.
        /// </summary>
        public virtual void Load()
        {
            string archiveStr = FileUtil.Instance.GetLocalFileText(
                FileUtil.LocalSourceType.PERSISTENT_DATA_PATH,
                ArchiveFileName
            );

            if (archiveStr == null)
            {
                Debug.LogWarning("Can not found archive file.");
                return;
            }

            Archive = JsonUtility.FromJson<ArchiveDataType>(archiveStr);
        }

        /// <summary>
        /// Save data to archive file.
        /// </summary>
        public virtual void Save()
        {
            string archiveStr = JsonUtility.ToJson(Archive);

            FileUtil.Instance.SetFile(
                ArchiveFileName,
                archiveStr
            );
        }

        /// <summary>
        /// Clear data.
        /// </summary>
        public void Clear()
        {
            Archive = new ArchiveDataType();
        }

        protected virtual string ArchiveFileName
        {
            get
            {
                return "archive_data.json";
            }
        }

        /// <summary>
        /// The archive of data.
        /// </summary>
        public ArchiveDataType Archive = new ArchiveDataType();
    }
}