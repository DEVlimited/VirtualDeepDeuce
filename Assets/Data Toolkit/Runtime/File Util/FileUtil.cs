using System;
using System.IO;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// IO operating system tools for basic files.
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// Indicates the source type of local file resource.
        /// </summary>
        public enum LocalSourceType
        {
            NONE,
            STREAMING_ASSETS,
            PERSISTENT_DATA_PATH
        }

        /// <summary>
        /// Indicates the source type of remote file resource.
        /// </summary>
        public enum RemoteSourceType
        { 
            NONE,
            FILE,
            ASSET_BUNDLE
        }

        /// <summary>
        /// Get local file content in synchronization.
        /// </summary>
        /// <param name="type">Local source type.</param>
        /// <param name="filePath">Relative path of the file.</param>
        /// <param name="fileBytes">Read file bytes.</param>
        /// <param name="fileText">Read file contents(in the form of string).</param>
        public void GetLocalFile(
            LocalSourceType type,
            string filePath,
            out byte[] fileBytes,
            out string fileText)
        {
            fileBytes = null;
            fileText = null;

            string platformFilePath = GetLocalFullPath(type, filePath);
            try
            {
                if (platformFilePath.IsLegalURI())
                {
                    DownloadRequest downloader = new DownloadRequest(platformFilePath);
                    while (!downloader.IsFinished) ;
                    fileBytes = downloader.DownloadedFileBytes;
                    fileText = downloader.DownloadedFileText;
                }
                else if (File.Exists(platformFilePath))
                {
                    fileBytes = File.ReadAllBytes(platformFilePath);
                    fileText = File.ReadAllText(platformFilePath);
                }
            }
            catch(Exception e)
            {
                string message = string.Format("Failed to get content of file: {0}. Exception: {1}", platformFilePath, e.Message);
                Debug.LogWarning(message);
            }
        }

        /// <summary>
        /// Get local file text in synchronization.
        /// </summary>
        /// <param name="type">Local source type.</param>
        /// <param name="filePath">Relative path of the file.</param>
        /// <returns>Read file contents(in the form of string).</returns>
        public string GetLocalFileText(LocalSourceType type, string filePath)
        {
            GetLocalFile(type, filePath, out byte[] bytes, out string fileText);
            return fileText;
        }

        /// <summary>
        /// Get local file bytes in synchronization.
        /// </summary>
        /// <param name="type">Local source type.</param>
        /// <param name="filePath">Relative path of the file.</param>
        /// <returns>Read file contents(in the form of string).</returns>
        public byte[] GetLocalFileBytes(LocalSourceType type, string filePath)
        {
            GetLocalFile(type, filePath, out byte[] fileBytes, out string fileText);
            return fileBytes;
        }

        /// <summary>
        /// Get remote file content in synchronization.
        /// </summary>
        /// <param name="type">Remote source type.</param>
        /// <param name="uri">URI of the download request.</param>
        /// <param name="assetBundle">Downloaded AssetBundle.</param>
        /// <param name="fileBytes">Downloaded file bytes.</param>
        /// <param name="fileText">Downloaded file contents(in the form of string).</param>
        /// <param name="version">
        /// An integer version number, which will be compared to the cached version of the asset bundle to download.
        /// Increment this number to force Unity to redownload a cached asset bundle.
        /// If zero, the version assignment is ignored.
        /// </param>
        /// <param name="crc">
        /// If nonzero, this number will be compared to the checksum of the downloaded asset bundle data.
        /// If the CRCs do not match, an error will be logged and the asset bundle will not be loaded.
        /// If set to zero, CRC checking will be skipped.
        /// </param>
        public void GetRemoteFile(
            RemoteSourceType type,
            string uri,
            out AssetBundle assetBundle,
            out byte[] fileBytes,
            out string fileText,
            uint version = 0,
            uint crc = 0)
        {
            assetBundle = null;
            fileBytes = null;
            fileText = null;

            try
            {
                if (!uri.IsLegalURI())
                    return;

                if (type == RemoteSourceType.FILE)
                {
                    DownloadRequest downloader = new DownloadRequest(uri);
                    while (!downloader.IsFinished);
                    fileBytes = downloader.DownloadedFileBytes;
                    fileText = downloader.DownloadedFileText;
                }
                else if (type == RemoteSourceType.ASSET_BUNDLE)
                {
                    DownloadRequest downloader = new DownloadRequest(uri, version, crc);
                    while (!downloader.IsFinished);
                    assetBundle = downloader.DownloadedAssetBundle;
                    fileBytes = downloader.DownloadedAssetBundleBytes;
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to get content of file: {0}. Exception: {1}", uri, e.Message);
                Debug.LogWarning(message);
            }
        }

        /// <summary>
        /// Get remote AssetBundle in synchronization.
        /// </summary>
        /// <param name="uri">URI of the download request.</param>
        /// <param name="version">
        /// An integer version number, which will be compared to the cached version of the asset bundle to download.
        /// Increment this number to force Unity to redownload a cached asset bundle.
        /// If zero, the version assignment is ignored.
        /// </param>
        /// <param name="crc">
        /// If nonzero, this number will be compared to the checksum of the downloaded asset bundle data.
        /// If the CRCs do not match, an error will be logged and the asset bundle will not be loaded.
        /// If set to zero, CRC checking will be skipped.
        /// </param>
        /// <returns>Downloaded Assetbundle.</returns>
        public AssetBundle GetRemoteAssetBundle(
            string uri,
            uint version = 0,
            uint crc = 0)
        {
            GetRemoteFile(RemoteSourceType.ASSET_BUNDLE, uri, out AssetBundle assetBundle, out byte[] fileBytes, out string fileText, version, crc);
            return assetBundle;
        }

        /// <summary>
        ///  Get remote file text in synchronization.
        /// </summary>
        /// <param name="uri">URI of the download request.</param>
        /// <returns>Downloaded text.</returns>
        public string GetRemoteFileText(string uri)
        {
            GetRemoteFile(RemoteSourceType.FILE, uri, out AssetBundle assetBundle, out byte[] fileBytes, out string fileText, 0, 0);
            return fileText;
        }

        /// <summary>
        /// Get remote file bytes in synchronization.
        /// </summary>
        /// <param name="type">Remote source type.</param>
        /// <param name="uri">URI of the download request.</param>
        /// <returns>Downloaded bytes.</returns>
        public byte[] GetRemoteFileBytes(LocalSourceType type, string uri)
        {
            GetLocalFile(type, uri, out byte[] bytes, out string fileStr);
            return bytes;
        }

        /// <summary>
        /// Get remote file bytes asynchronously.
        /// </summary>
        /// <param name="uri">URI of the download request.</param>
        /// <param name="respondCallback">Callback for downloading results.</param>
        /// <returns>Handle of download.</returns>
        public DownloadRequest GetRemoteFileAsync(
            string uri,
            DownloadRequest.OnFileDownloadFinished respondCallback = null
        )
        {
            try
            {
                if (uri.IsLegalURI())
                {
                    DownloadRequest downloader = new DownloadRequest(uri, respondCallback);
                    return downloader;
                }
                else
                {
                    Debug.LogError(string.Format("{0} is not a legal remote uri.", uri));
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to get content in file: {0}. Exception: {1}", uri, e.Message);
                Debug.LogWarning(message);
            }

            return null;
        }

        /// <summary>
        /// Get remote AssetBundle asynchronously.
        /// </summary>
        /// <param name="uri">URI of the download request.</param>
        /// <param name="version">
        /// An integer version number, which will be compared to the cached version of the asset bundle to download.
        /// Increment this number to force Unity to redownload a cached asset bundle.
        /// If zero, the version assignment is ignored.
        /// </param>
        /// <param name="crc">
        /// If nonzero, this number will be compared to the checksum of the downloaded asset bundle data.
        /// If the CRCs do not match, an error will be logged and the asset bundle will not be loaded.
        /// If set to zero, CRC checking will be skipped.
        /// </param>
        /// <param name="respondCallback">Callback for downloading results.</param>
        /// <returns>Handle of download.</returns>
        public DownloadRequest GetRemoteFileAsync(
            string uri,
            uint version,
            uint crc = 0,
            DownloadRequest.OnAssetBundleDownloadFinished respondCallback = null
        )
        {
            try
            {
                if (uri.IsLegalURI())
                {
                    DownloadRequest downloader = new DownloadRequest(uri, version, crc, respondCallback);
                    return downloader;
                }
                else
                {
                    Debug.LogError(string.Format("{0} is not a legal remote uri.", uri));
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to get content in file {0}. Exception: {1}", uri, e.Message);
                Debug.LogWarning(message);
            }

            return null;
        }

        /// <summary>
        /// Write the specified text to the file of persistent data path.
        /// If the file does not exist, it will be created.
        /// </summary>
        /// <param name="filePath">The relative path of the file.</param>
        /// <param name="textContent">Text content.</param>
        /// <param name="addOnMode">Enable append mode or not.</param>
        public void SetFile(string filePath, string textContent, bool addOnMode = false)
        {
            try
            {
                string recordFilePath = Application.persistentDataPath + "/" + filePath;
                string recordFolderPath = Path.GetDirectoryName(recordFilePath);
                if (!Directory.Exists(recordFolderPath))
                    Directory.CreateDirectory(recordFolderPath);

                if (!File.Exists(recordFilePath))
                { 
                    FileStream fs = File.Create(recordFilePath);
                    fs.Close();
                }

                if (addOnMode)
                {
                    File.AppendAllText(recordFilePath, textContent);
                }
                else
                {
                    File.WriteAllText(recordFilePath, textContent);
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to set text in file {0}. Exception: {1}", filePath, e.Message);
                Debug.LogWarning(message);
            }
        }

        /// <summary>
        /// Write the specified bytes to the file of persistent data path.
        /// If the file does not exist, it will be created.
        /// </summary>
        /// <param name="filePath">The relative path of the file.</param>
        /// <param name="bytesContent">Bytes content.</param>
        public void SetFile(string filePath, byte[] bytesContent)
        {
            try
            {
                string recordFilePath = Application.persistentDataPath + "/" + filePath;
                string recordFolderPath = Path.GetDirectoryName(recordFilePath);
                if (!Directory.Exists(recordFolderPath))
                    Directory.CreateDirectory(recordFolderPath);

                if (!File.Exists(recordFilePath))
                {
                    FileStream fs = File.Create(recordFilePath);
                    fs.Close();
                }
                File.WriteAllBytes(recordFilePath, bytesContent);
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to set bytes in file {0}. Exception: {1}", filePath, e.Message);
                Debug.LogWarning(message);
            }
        }

        private string GetLocalFullPath(LocalSourceType type, string filePath)
        {
            if (type == LocalSourceType.STREAMING_ASSETS)
            {
                string prefixStr = Application.streamingAssetsPath + "/";
                if (filePath.StartsWith(prefixStr))
                    return filePath;
                return prefixStr + filePath;
            }
            else if (type == LocalSourceType.PERSISTENT_DATA_PATH)
            {
                string prefixStr = Application.persistentDataPath + "/";
                if (filePath.StartsWith(prefixStr))
                    return filePath;
                return prefixStr + filePath;
            }
            else
            {
                return filePath;
            }
        }

        private FileUtil() {}

        /// <summary>
        /// Access interface of singleton design pattern object.
        /// </summary>
        public static FileUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syslock)
                    {
                        if (instance == null)
                        {
                            instance = new FileUtil();
                        }
                    }
                }
                return instance;
            }
        }

        private static FileUtil instance;
        private static readonly object syslock = new object();
    }
}