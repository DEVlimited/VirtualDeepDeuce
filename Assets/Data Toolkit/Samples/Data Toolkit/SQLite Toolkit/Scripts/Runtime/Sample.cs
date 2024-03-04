using Mono.Data.Sqlite;
using UnityEngine;
using Arcspark.DataToolkit;

namespace Arcspark.Sample.SQLiteToolkitSample
{
    public class Sample : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        // Sample Use SQLite Helper
        private void Init()
        {
            SQLiteConnection sqliteDB = new SQLiteConnection(DBConnectString);

            PrintTableUnit(sqliteDB);
            AddNewTable(sqliteDB);
            InsertSomeAccount(sqliteDB);
            DeleteSomeAccount0(sqliteDB);
            DeleteSomeAccount1(sqliteDB);
            UpdateSomeAccount(sqliteDB);
            DeleteTable(sqliteDB);

            sqliteDB.Close();
        }

        private void AddNewTable(SQLiteConnection sqliteDB)
        {
            sqliteDB.CreateTable("Unit1",
                new string[] { "integer NOT NULL", "text", "real", "text" },
                new string[] { "Card_Number", "Card_Name", "Balance", "Certificate_Timestamp" }
            );

            Debug.Log("== Add New Table Finished ==");
        }

        private void InsertSomeAccount(SQLiteConnection sqliteDB)
        {
            sqliteDB.InsertValues("Unit1", new string[] { "10000", "'Visa'", "2021.91", "'2010-1-31'" });
            sqliteDB.InsertValues("Unit1", new string[] { "20000", "'Master'", "1.32", "'2001-10-1'" });
            sqliteDB.InsertValues("Unit1", new string[] { "30000", "'Discover'", "-101.32", "'2008-12-1'" });
            sqliteDB.InsertValues("Unit1", new string[] { "40000", "'JCB'", "9001", "'2004-8-22'" });
            sqliteDB.InsertValues("Unit1", new string[] { "50000", "'AmericanExpress'", "1300.2", "'2012-7-1'" });

            Debug.Log("== Insert New Values Finished ==");

            PrintTableUnit1(sqliteDB);
        }

        private void DeleteSomeAccount0(SQLiteConnection sqliteDB)
        {
            sqliteDB.DeleteValuesOR("Unit1",
                new string[] { "Balance", "Card_Name" },
                new string[] { "<", "=" },
                new string[] { "0", "'AmericanExpress'" }
            );

            Debug.Log("== Delete Values Finished: Part 0==");

            PrintTableUnit1(sqliteDB);
        }

        private void DeleteSomeAccount1(SQLiteConnection sqliteDB)
        {
            sqliteDB.DeleteValuesAND("Unit1",
                new string[] { "Balance", "Certificate_Timestamp" },
                new string[] { ">=", "<>" },
                new string[] { "2000", "'2004-8-22'" }
            );

            Debug.Log("== Delete Values Finished: Part 1 ==");

            PrintTableUnit1(sqliteDB);
        }

        private void UpdateSomeAccount(SQLiteConnection sqliteDB)
        {
            sqliteDB.UpdateValuesAND("Unit1",
                new string[] {"Card_Number"},
                new string[] {"="},
                new string[] {"20000"},
                new string[] { "Card_Number", "Card_Name", "Balance" },
                new string[] { "0", "N/A", "0" }
            );

            Debug.Log("== Update Values Finished ==");

            PrintTableUnit1(sqliteDB);
        }

        private void DeleteTable(SQLiteConnection sqliteDB)
        {
            sqliteDB.DropTable("Unit1");

            Debug.Log("== Delete Table Finished ==");
        }

        private void PrintTableUnit(SQLiteConnection sqliteDB)
        {
            Debug.Log("== Print DB Unit ==");

            SqliteDataReader reader = sqliteDB.SelectTable("Unit");                                                                                                                                                                                                                                   
            do
            {
                while (reader.Read())
                {
                    try
                    {
                        int? id = reader.GetInt32("ID");
                        string name = reader.GetString("Name");
                        double? attackDamage = reader.GetDouble("Attack_Damage");

                        Debug.Log(string.Format("ID: {0}, Name: {1}, AttackDamage: {2}", id, name, attackDamage));
                    }
                    catch (SqliteException e)
                    {
                        Debug.Log(e.Message);
                    }
                }
            }
            while (reader.NextResult());
        }

        private void PrintTableUnit1(SQLiteConnection sqliteDB)
        {
            Debug.Log("== Print DB Unit1 ==");

            SqliteDataReader reader = sqliteDB.SelectTable("Unit1");
            do
            {
                while (reader.Read())
                {
                    try
                    {
                        int? cardNumber = reader.GetInt32("Card_Number");
                        string cardName = reader.GetString("Card_Name");
                        double? balance = reader.GetDouble("Balance");
                        string certificateTimestamp = reader.GetString("Certificate_Timestamp");

                        Debug.Log(string.Format("Card_Number: {0}, Card_Name: {1}, Balance: {2}, Certificate_Timestamp: {3}", cardNumber, cardName, balance, certificateTimestamp));
                    }
                    catch (SqliteException e)
                    {
                        Debug.Log(e.Message);
                    }
                }
            }
            while (reader.NextResult());
        }

        // Get Database from StreamingAssets Folder
        private string DBConnectString
        {
            get => Application.streamingAssetsPath+ "/Samples/Data Toolkit/test.db";
        }
    }
}