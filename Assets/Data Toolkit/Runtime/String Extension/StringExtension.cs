using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// Extension of the System.String.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// For the string file path, replace the char '\' in all strings with the char '/'.
        /// </summary>
        /// <param name="pathStr">Original string.</param>
        /// <returns>Modified string.</returns>
        public static string ToUnityPath(this string pathStr)
        {
            return pathStr.Replace('\\', '/');
        }

        /// <summary>
        /// Determine whether the given string is a legal URI.
        /// </summary>
        /// <param name="uri">Original string.</param>
        /// <returns>
        /// Returns true if the given string is a legal URI,
        /// otherwise returns false
        /// </returns>
        public static bool IsLegalURI(this string uri)
        {
            return !string.IsNullOrEmpty(uri) && uri.Contains("://");
        }

        /// <summary>
        /// Determine whether the given string is a legal HTTP URI.
        /// </summary>
        /// <param name="uri">Original string.</param>
        /// <returns>
        /// Returns true if the given string is a legal HTTP URI,
        /// otherwise returns false
        /// </returns>
        public static bool IsLegalHTTPURI(this string uri)
        {
            uri = uri.ToLower();
            return !string.IsNullOrEmpty(uri) && (uri.StartsWith("http://") || uri.StartsWith("https://"));
        }
    }
}