using System;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// Provide data caching function.
    /// </summary>
    public class CacheUtil
    {
        /// <summary>
        /// Get a cached value by specifying the key.
        /// </summary>
        /// <param name="key">A cached key.</param>
        /// <param name="value">If the cached value is obtained, it will be returned through value.</param>
        /// <returns>
        /// Returns ture if the value in the cache is successfully obtained;
        /// otherwise, it returns false.
        /// </returns>
        public bool GetValue<T>(string key, out T value)
        {
            value = default(T);
            try
            {
                if (value is bool)
                    value = (T)(object)Convert.ToBoolean(PlayerPrefs.GetInt(key));
                else if (value is int)
                    value = (T)(object)PlayerPrefs.GetInt(key);
                else if (value is float)
                    value = (T)(object)PlayerPrefs.GetFloat(key);
                else if (value is string)
                    value = (T)(object)PlayerPrefs.GetString(key);
                else
                    value = (T)(object)PlayerPrefs.GetString(key);
                return true;
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to convert value: {0}. Exception: {1}", value.ToString(), e.Message);
                Debug.Log(message);
                return false;
            }
        }

        /// <summary>
        /// Set a cached value by specifying the key.
        /// </summary>
        /// <param name="key">A key to cache.</param>
        /// <param name="value">A value to cache.</param>
        /// <returns>
        /// Returns ture if the value in the cache is successfully cached;
        /// otherwise, it returns false.
        /// </returns>
        public bool SetValue<T>(string key, T value)
        {
            try
            {
                if (value is bool)
                    PlayerPrefs.SetInt(key, Convert.ToInt32(value));
                else if (value is int)
                    PlayerPrefs.SetInt(key, (int)(object)value);
                else if (value is float)
                    PlayerPrefs.SetFloat(key, (float)(object)value);
                else if (value is string)
                    PlayerPrefs.SetString(key, (string)(object)value);
                else
                    PlayerPrefs.SetString(key, value.ToString());
                return true;
            }
            catch (Exception e)
            {
                string message = string.Format("Failed to convert value: {0}. Exception: {1}", value.ToString(), e.Message);
                Debug.Log(message);
                return false;
            }
        }

        /// <summary>
        /// Delete a cached value by specifying the key.
        /// </summary>
        /// <param name="key">The key corresponding to the value to be deleted.</param>
        /// <returns>
        /// Returns ture if the value in the cache is successfully deleted;
        /// otherwise, it returns false.
        /// </returns>
        public bool DeleteValue(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Delete all cached values.
        /// </summary>
        public void ClearCache()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Write all modifield preferences to disk,
        /// </summary>
        public void SaveCache()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Access interface of singleton mode object.
        /// </summary>
        public static CacheUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syslock)
                    {
                        if (instance == null)
                        {
                            instance = new CacheUtil();
                        }
                    }
                }
                return instance;
            }
        }

        private CacheUtil() {}

        private static CacheUtil instance;
        private static readonly object syslock = new object();
    }
}