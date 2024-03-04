using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// SQL related auxiliary tools.
    /// </summary>
    public static class SQLUtil
    {
        /// <summary>
        /// Create a new empty database file in persistent data path.
        /// </summary>
        /// <param name="path">Relative path of the target database file.</param>
        /// <param name="dbFileExtensionName">Extension name of database file.</param>
        /// <returns>A SQLite connect string for SQLiteConnection.</returns>
        public static string CreateSQLiteDatabase(string path, string dbFileExtensionName = ".db")
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path.StartsWith('/'))
                path.Remove(0, 1);
            if (path.EndsWith('/'))
                path.Remove(path.Length - 1, 1);

            if (!path.EndsWith(dbFileExtensionName))
                path += dbFileExtensionName;

            byte[] bytes = EmptyDB;
            if (bytes != null)
            {
                FileUtil.Instance.SetFile(path, EmptyDB);
                return Application.persistentDataPath + "/" + path;
            }

            return null;
        }

        private static byte[] EmptyDB
        {
            get
            {
                if (emptyDB != null)
                    return emptyDB;

                TextAsset ta = Resources.Load<TextAsset>(emptyDBResPath);
                if (ta != null)
                    emptyDB = ta.bytes;

                if (emptyDB == null)
                {
                    Debug.Log(
                        string.Format(
                            "Cannot load asset {0} from Resources. Please check it.", emptyDBResPath));
                }

                return emptyDB;
            }
        }

        private static byte[] emptyDB;
        private const string emptyDBResPath = "Data/Empty DB";
    }
}