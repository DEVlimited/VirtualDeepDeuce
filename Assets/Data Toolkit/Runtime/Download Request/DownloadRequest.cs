using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// A resource file downloader.
    /// Use UWR to provide the download function of network content.
    /// </summary>
    public class DownloadRequest
    {
        /// <summary>
        /// A delegate called when the download of a common network resource file is completed.
        /// </summary>
        /// <param name="success">
        /// If the download is successful, it returns true;
        /// otherwise, it returns false.
        /// </param>
        /// <param name="bytes">Downloaded file bytes.</param>
        /// <param name="text">Downloaded file text.</param>
        public delegate void OnFileDownloadFinished(bool success, byte[] bytes, string text);

        /// <summary>
        /// A delegate called when the download of a common type of network Assetbundle file is completed.
        /// </summary>
        /// <param name="success">
        /// If the download is successful, it returns true;
        /// otherwise, it returns false.
        /// </param>
        /// <param name="bytes">Downloaded Assetbundle bytes.</param>
        /// <param name="ab">Downloaded Assetbundle object.</param>
        public delegate void OnAssetBundleDownloadFinished(bool success, byte[] bytes, AssetBundle ab);

        private enum DownloadType
        {
            NONE,
            FILE,
            ASSET_BUNDLE
        }

        /// <summary>
        /// Create a file download request.
        /// </summary>
        /// <param name="uri">Requested URI.</param>
        /// <param name="respondCallback">Download complete callback.</param>
        public DownloadRequest(string uri, OnFileDownloadFinished respondCallback = null)
        {
            type = DownloadType.FILE;
            fileDownLoadCallback += respondCallback;
            SendFileDownloadRequest(uri);
        }

        /// <summary>
        /// Create a AssetBundle download request
        /// </summary>
        /// <param name="uri">Requested URI.</param>
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
        /// <param name="respondCallback">Download complete callback.</param>
        public DownloadRequest(
            string uri,
            uint version,
            uint crc = 0,
            OnAssetBundleDownloadFinished respondCallback = null)
        {
            type = DownloadType.ASSET_BUNDLE;
            assetBundleDownLoadCallback += respondCallback;
            SendAssetBundleDownloadRequest(uri, version, crc);
        }

        /// <summary>
        /// Abort the ongoing download.
        /// </summary>
        public void Abort()
        {
            if(!IsFinished)
                uwr.Abort();
        }

        private void SendFileDownloadRequest(string uri)
        {
            try
            {
                if (uri.IsLegalURI())
                {
                    uwr = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET, new DownloadHandlerBuffer(), null);
                    var rq = uwr.SendWebRequest();
                    rq.completed += OnRequestOpCompleted;
                }
                else
                    loadFail();
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to send file download request for uri: {0}. Exception: {1}", uri, e.Message);
                Debug.LogWarning(message);
                loadFail();
            }
        }

        private void SendAssetBundleDownloadRequest(string uri, uint version = 0, uint crc = 0)
        {
            try
            {
                if (uri.IsLegalURI())
                {
                    if (version == 0)
                        uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri);
                    else
                        uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri, version, crc);
                    var rq = uwr.SendWebRequest();
                    rq.completed += OnRequestOpCompleted;
                }
                else
                    loadFail();
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to asset bundle download request for uri: {0}. Exception: {1}", uri, e.Message);
                Debug.LogWarning(message);
                loadFail();
            }
        }

        private void OnRequestOpCompleted(AsyncOperation op)
        {
            try
            {
                switch (type)
                {
                    case DownloadType.FILE:
                        loadSuccess(
                            uwr.downloadHandler.data,
                            uwr.downloadHandler.text);
                        break;
                    case DownloadType.ASSET_BUNDLE:
                        loadSuccess(
                            uwr.downloadHandler.data,
                            DownloadHandlerAssetBundle.GetContent(uwr));
                        break;
                    default:
                        string message = string.Format("Failed to completed request send file download request : {0} due to download type error. DownloadType is: {1}", op.ToString(), type.ToString());
                        Debug.LogError(message);
                        loadFail();
                        break;
                }
                uwr.Dispose();
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to completed request send file download request : {0}. Exception: {1}", op.ToString(), e.Message);
                Debug.LogWarning(message);
                loadFail();
            }
        }

        private void loadFail()
        {
            switch (type)
            {
                case DownloadType.FILE:
                    if (fileDownLoadCallback != null)
                        fileDownLoadCallback(false, null, null);
                    break;
                case DownloadType.ASSET_BUNDLE:
                    if (assetBundleDownLoadCallback != null)
                        assetBundleDownLoadCallback(false, null, null);
                    break;
            }
        }

        private void loadSuccess(byte[] bytes, string text)
        {
            if (fileDownLoadCallback != null)
                fileDownLoadCallback(true, bytes, text);
        }

        private void loadSuccess(byte[] bytes, AssetBundle ab)
        {
            if (assetBundleDownLoadCallback != null)
                assetBundleDownLoadCallback(true, bytes, ab);
        }

        /// <summary>
        /// Returns true if the request is complete, otherwise returns false.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                if (uwr == null)
                    return true;
                else
                    return uwr.isDone;
            }
        }

        /// <summary>
        /// Requested download progress.
        /// </summary>
        public float Progress
        {
            get
            {
                if (uwr != null)
                    return uwr.downloadProgress;
                return 0f;
            }
        }

        /// <summary>
        /// Get the downloaded bytes.
        /// It returns the corresponding value according to the request type.
        /// </summary>
        public byte[] DownloadedBytes
        {
            get
            {
                if (!IsFinished)
                    return null;

                if ((type == DownloadType.FILE ||
                    type == DownloadType.ASSET_BUNDLE) &&
                    uwr != null)
                    return uwr.downloadHandler.data;

                return null;
            }
        }

        /// <summary>
        /// Get the downloaded file bytes.
        /// </summary>
        public byte[] DownloadedFileBytes
        {
            get
            {
                if (!IsFinished)
                    return null;

                if (type == DownloadType.FILE &&
                    uwr != null)
                    return uwr.downloadHandler.data;

                return null;
            }
        }

        /// <summary>
        /// Get the downloaded AssetBundle.
        /// </summary>
        public AssetBundle DownloadedAssetBundle
        {
            get
            {
                if (!IsFinished)
                    return null;

                if (type == DownloadType.ASSET_BUNDLE &&
                    uwr != null)
                    return DownloadHandlerAssetBundle.GetContent(uwr);

                return null;
            }
        }

        /// <summary>
        /// Get the downloaded file text.
        /// </summary>
        public string DownloadedFileText
        {
            get
            {
                if (!IsFinished)
                    return null;

                if (type == DownloadType.FILE &&
                    uwr != null)
                    return uwr.downloadHandler.text;

                return null;
            }
        }

        /// <summary>
        /// Get the downloaded AssetBundle bytes.
        /// </summary>
        public byte[] DownloadedAssetBundleBytes
        {
            get
            {
                if (!IsFinished)
                    return null;

                if (type == DownloadType.ASSET_BUNDLE &&
                    uwr != null)
                    return uwr.downloadHandler.data;

                return null;
            }
        }

        private DownloadType type;
        private UnityWebRequest uwr;
        private event OnFileDownloadFinished fileDownLoadCallback;
        private event OnAssetBundleDownloadFinished assetBundleDownLoadCallback;
    }
}