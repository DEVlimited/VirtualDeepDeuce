using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// Extension of get value method of SqliteDataReader.
    /// </summary>
    public static class SqliteDataReaderExtension
    {
        public static bool? GetBoolean(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetBoolean(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static byte? GetByte(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetByte(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static char? GetChar(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetChar(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static string GetDataTypeName(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetDataTypeName(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static DateTime? GetDateTime(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetDateTime(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static decimal? GetDecimal(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetDecimal(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static double? GetDouble(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetDouble(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static Type GetFieldType(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetFieldType(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static T GetFieldValue<T>(this SqliteDataReader reader, string name)
            where T: class
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return default(T);
                return reader.GetFieldValue<T>(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return default(T);
        }

        public static float? GetFloat(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetFloat(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static Guid? GetGuid(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetGuid(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static short? GetInt16(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;
                return reader.GetInt16(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static int? GetInt32(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetInt32(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static long? GetInt64(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetInt64(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static string GetName(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetName(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static Stream GetStream(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetStream(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static string GetString(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetString(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static TextReader GetTextReader(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetTextReader(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }

        public static object GetValue(this SqliteDataReader reader, string name)
        {
            try
            {
                int idx = reader.GetOrdinal(name);
                if (reader.IsDBNull(idx))
                    return null;

                return reader.GetValue(idx);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return null;
        }
    }
}