using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace management_system
{
    public class Database
    {
        public SQLiteConnection Connection;

        public Database()
        {
            if (!File.Exists("../../../database.sqlite"))
            {
                SQLiteConnection.CreateFile("../../../database.sqlite");
            }
            string createUserTable = "create table users (id INTEGER, name TEXT, surname TEXT, email TEXT);";
            string createWorkersTable =
                "create table workers (id INTEGER, name TEXT, surname TEXT, salary INTEGER, email TEXT);";
            Connection = new SQLiteConnection("Data Source=../../../database.sqlite");
            string getTablesQuery =
                "SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
            
            List<string> tables = new List<string>();
            SQLiteCommand getTables = new SQLiteCommand(getTablesQuery, Connection);
            Connection.Open();
            SQLiteDataReader reader = getTables.ExecuteReader();
            while (reader.Read())
                tables.Add(Convert.ToString(reader["name"]));
            if (!tables.Contains("users"))
            {
                SQLiteCommand addUserTab = new SQLiteCommand(createUserTable, Connection);
                addUserTab.ExecuteNonQuery();
            }

            if (!tables.Contains("workers"))
            {
                SQLiteCommand addWorkersTab = new SQLiteCommand(createWorkersTable, Connection);
                addWorkersTab.ExecuteNonQuery();
            }

            Connection.Close();
        }
    }
}