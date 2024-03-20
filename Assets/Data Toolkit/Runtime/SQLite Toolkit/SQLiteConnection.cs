using Mono.Data.Sqlite;
using System;
using UnityEngine;

namespace Arcspark.DataToolkit
{
    /// <summary>
    /// Manage a database connection and provide transaction-safe CRUD services.
    /// </summary>
    public class SQLiteConnection
    {
        /// <summary>
        /// Construct and connect to the specific database.
        /// </summary>
        /// <param name="connectionString">
        /// Database connection string.
        /// A connection string is used to specify how to connect to the database.
        /// Connection strings follow the standard ADO.NET syntax as a semicolon-separated list of keywords and values.
        /// </param>
        public SQLiteConnection(string connectionString)
        {
            if (!connectionString.StartsWith("URI=file:"))
                connectionString = "URI=file:" + connectionString;

            try
            {
                // Open database connection.
                dbConnection = new SqliteConnection(connectionString);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Close the database connection.
        /// </summary>
        public void Close()
        {
            // Destroy command.
            if (dbCommand != null)
            {
                try
                {
                    dbCommand.Cancel();
                }
                catch(Exception e) 
                {
                    Debug.LogError(e.Message);
                }
            }
            dbCommand = null;

            // Destroy Reader.
            if (dataReader != null)
            {
                try
                {
                    dataReader.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            dataReader = null;

            //Destroy Connection.
            if (dbConnection != null)
            {
                try
                {
                    dbConnection.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            dbConnection = null;
        }

        /// <summary>
        /// Create a table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="types">Field types.</param>
        /// <param name="keys">Field names.</param>
        /// <returns>Created table.</returns>
        public SqliteDataReader CreateTable(string tableName, string[] types, string[] keys)
        {
            if (keys.Length == 0 ||
                types.Length == 0 ||
                types.Length != keys.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase CreateTable: The creation parameter is incorrect.");
            }

            string queryString = "CREATE TABLE " + RegularParam(tableName) + "( " + RegularParam(keys[0]) + " " + types[0];
            for (int i = 1; i < keys.Length; ++i)
            {
                queryString += (", " + RegularParam(keys[i]) + " " + types[i]);
            }
            queryString += "  )";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Drop a table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        public SqliteDataReader DropTable(string tableName)
        {
            string queryString = "DROP TABLE " + RegularParam(tableName);
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Excute the specific SQL query.
        /// </summary>
        /// <param name="queryString">SQL query string.</param>
        /// <returns>Result of SQL query.</returns>
        public SqliteDataReader ExecuteQuery(string queryString)
        {
            using (dbCommand = dbConnection.CreateCommand())

            using (var transaction = dbConnection.BeginTransaction())
            {
                try
                {
                    // Close the connection before reading new one.
                    dataReader?.Close();
                    // Execute database operations.
                    dbCommand.CommandText = queryString;
                    dataReader = dbCommand.ExecuteReader();
                    // Commit transaction.
                    transaction.Commit();
                }
                catch (SqliteException e)
                {
                    // Rollback transaction.
                    transaction.Rollback();
                    Debug.LogWarning(e);
                }

                return dataReader;
            }
        }

        /// <summary>
        /// Inserts data into the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="values">Inserted values.</param>
        public SqliteDataReader InsertValues(string tableName, string[] values)
        {
            // Gets the number of fields in the data table.
            int fieldCount = SelectTable(tableName).FieldCount;
            if (values.Length == 0 ||
                values.Length != fieldCount
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase InsertValues: The insert parameter is incorrect.");
            }

            string queryString = "INSERT INTO " + RegularParam(tableName) + " VALUES (" + RegularParam(values[0]);
            for (int i = 1; i < values.Length; ++i)
            {
                queryString += ", " + RegularParam(values[i]);
            }
            queryString += " )";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Delete data in the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="whereClause">Where clause.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader DeleteValues(string tableName, string whereClause)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (string.IsNullOrEmpty(whereClause))
            {
                throw new SqliteException("Exception from SQLiteDatabase DeleteValuesOR: The delete parameter is incorrect.");
            }

            string queryString = "DELETE FROM " + RegularParam(tableName) + " WHERE " + whereClause;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Delete data in the specified table.
        /// The where clause is connected with OR.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="condKeys">Field names for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Field values for judgment.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader DeleteValuesOR(string tableName, string[] condKeys, string[] operations, string[] condValues)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (condKeys.Length == 0 ||
                operations.Length == 0 ||
                operations.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase DeleteValuesOR: The delete parameter is incorrect.");
            }

            string queryString = "DELETE FROM " + RegularParam(tableName) + " WHERE " + RegularParam(condKeys[0]) + operations[0] + RegularParam(condValues[0]);
            for (int i = 1; i < condValues.Length; ++i)
            {
                queryString += " OR " + RegularParam(condKeys[i]) + operations[i] + RegularParam(condValues[i]);
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Delete data in the specified table.
        /// The where clause is connected with AND.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="condKeys">Field names for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Field values for judgment.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader DeleteValuesAND(string tableName, string[] condKeys, string[] operations, string[] condValues)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (condKeys.Length == 0 ||
                operations.Length == 0 ||
                condValues.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase DeleteValuesOR: The delete parameter is incorrect.");
            }

            string queryString = "DELETE FROM " + RegularParam(tableName) + " WHERE " + RegularParam(condKeys[0]) + operations[0] + RegularParam(condValues[0]);
            for (int i = 1; i < condValues.Length; ++i)
            {
                queryString += " AND " + RegularParam(condKeys[i]) + operations[i] + RegularParam(condValues[i]);
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Update data in the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="keys">Field names for update.</param>
        /// <param name="values">Field values for update.</param>
        /// <param name="whereClause">Where clause.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader UpdateValues(string tableName, string[] keys, string[] values, string whereClause)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (keys.Length == 0 ||
                values.Length == 0 ||
                keys.Length != values.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "UPDATE " + RegularParam(tableName) + " SET " + RegularParam(keys[0]) + "=" + RegularParam(values[0]);
            for (int i = 1; i < values.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]) + "=" + RegularParam(values[i]);
            }
            queryString += " WHERE " + whereClause;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Update data in the specified table.
        /// The where clause is connected with OR.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="condKeys">Field names for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Fields value for judgment.</param>
        /// <param name="keys">Field names for update.</param>
        /// <param name="values">Field values for update.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader UpdateValuesOR(string tableName, string[] condKeys, string[] operations, string[] condValues, string[] keys, string[] values)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (condKeys.Length == 0 ||
                operations.Length == 0 ||
                condValues.Length == 0 ||
                keys.Length == 0 ||
                values.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length ||
                keys.Length != values.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "UPDATE " + RegularParam(tableName) + " SET " + RegularParam(keys[0]) + "=" + RegularParam(values[0]);
            for (int i = 1; i < values.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]) + "=" + RegularParam(values[i]);
            }
            queryString += " WHERE " + RegularParam(condKeys[0]) + operations[0] + RegularParam(condValues[0]);
            for (int i = 1; i < condValues.Length; ++i)
            {
                queryString += " OR " + RegularParam(condKeys[i]) + operations[i] + RegularParam(condValues[i]);
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Update data in the specified table.
        /// The where clause is connected with AND.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="condKeys">Field names for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Field values for judgment.</param>
        /// <param name="keys">Field names for update.</param>
        /// <param name="values">Field values for update.</param>
        /// <returns>Updated table.</returns>
        public SqliteDataReader UpdateValuesAND(string tableName, string[] condKeys, string[] operations, string[] condValues, string[] keys, string[] values)
        {
            // An SQL exception is thrown when the field name and field value do not correspond.
            if (condKeys.Length == 0 ||
                operations.Length == 0 ||
                condValues.Length == 0 ||
                keys.Length == 0 ||
                values.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length ||
                keys.Length != values.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "UPDATE " + RegularParam(tableName) + " SET " + RegularParam(keys[0]) + "=" + RegularParam(values[0]);
            for (int i = 1; i < values.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]) + "=" + RegularParam(values[i]);
            }
            queryString += " WHERE " + RegularParam(condKeys[0]) + operations[0] + RegularParam(condValues[0]);
            for (int i = 1; i < condValues.Length; ++i)
            {
                queryString += " AND " + RegularParam(condKeys[i]) + operations[i] + RegularParam(condValues[i]);
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Select the whole table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>Read table.</returns>
        public SqliteDataReader SelectTable(string tableName)
        {
            string queryString = "SELECT * FROM " + RegularParam(tableName);
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Select data in the specified table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="keys">Field values for select.</param>
        /// <param name="whereClause">Where clause.</param>
        /// <returns>Read table.</returns>
        public SqliteDataReader SelectTable(string tableName, string[] keys, string whereClause)
        {
            if (keys.Length == 0)
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "SELECT " + RegularParam(keys[0]);
            for (int i = 1; i < keys.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]);
            }
            queryString += " FROM " + RegularParam(tableName) + " WHERE " + whereClause;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Select data in the specified table.
        /// The where clause is connected with OR.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="keys">Field values for select.</param>
        /// <param name="condKeys">Field values for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Field values for judgment.</param>
        /// <returns>Read table.</returns>
        public SqliteDataReader SelectTableOR(string tableName, string[] keys, string[] condKeys, string[] operations, string[] condValues)
        {
            if (keys.Length == 0 ||
                condKeys.Length == 0 ||
                condValues.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "SELECT " + RegularParam(keys[0]);
            for (int i = 1; i < keys.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]);
            }
            queryString += " FROM " + RegularParam(tableName) + " WHERE " + RegularParam(condKeys[0]) + " " + operations[0] + " " + RegularParam(condValues[0]);
            for (int i = 0; i < condKeys.Length; ++i)
            {
                queryString += " OR " + RegularParam(condKeys[i]) + " " + operations[i] + " " + RegularParam(condValues[i]) + " ";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Select data in the specified table.
        /// The where clause is connected with AND.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="keys">Field values for select.</param>
        /// <param name="condKeys">Field values for judgment.</param>
        /// <param name="operations">Operators for judgment.</param>
        /// <param name="condValues">Field values for judgment.</param>
        /// <returns>Read table.</returns>
        public SqliteDataReader SelectTableAND(string tableName, string[] keys, string[] condKeys, string[] operations, string[] condValues)
        {
            if (keys.Length == 0 ||
                condKeys.Length == 0 ||
                condValues.Length == 0 ||
                condKeys.Length != condValues.Length ||
                operations.Length != condKeys.Length
            )
            {
                throw new SqliteException("Exception from SQLiteDatabase UpdateValues: The update parameter is incorrect.");
            }

            string queryString = "SELECT " + RegularParam(keys[0]);
            for (int i = 1; i < keys.Length; ++i)
            {
                queryString += ", " + RegularParam(keys[i]);
            }
            queryString += " FROM " + RegularParam(tableName) + " WHERE " + RegularParam(condKeys[0]) + " " + operations[0] + " " + RegularParam(condValues[0]);
            for (int i = 0; i < condKeys.Length; ++i)
            {
                queryString += " AND " + RegularParam(condKeys[i]) + " " + operations[i] + " " + RegularParam(condValues[i]) + " ";
            }
            return ExecuteQuery(queryString);
        }

        private string RegularParam(string str)
        {
            if (str == null)
                return null;

            str = str.Replace("'", "\"");

            if (str == "" ||
                str.Contains(" ") ||
                str.Contains("'") ||
                str.Contains("\"") ||
                str.Contains("/") ||
                str.Contains("&") ||
                str.Contains("||") ||
                str.Contains("@") ||
                str.Contains("%") ||
                str.Contains("*") ||
                str.Contains("(") ||
                str.Contains(")") ||
                str.Contains("-") ||
                str.Contains("_") ||
                str.Contains(","))
            { 
                return "'" + str + "'";
            }

            return str;
        }

        private SqliteConnection dbConnection;
        private SqliteDataReader dataReader;
        private SqliteCommand dbCommand;
    }
}