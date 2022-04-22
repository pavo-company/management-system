using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace management_system
{
    public class Database
    {
        private const string DatabasePath = "../../../database.sqlite";
        public SQLiteConnection Connection;

        public Database()
        {
            if (!File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
            }
            string createUserTable = "create table users (id PRIMARY KEY, name TEXT, surname TEXT, email TEXT);";
            string createWorkersTable =
                "create table workers (id PRIMARY KEY, name TEXT, surname TEXT, salary INTEGER, email TEXT);";
            Connection = new SQLiteConnection($"Data Source={DatabasePath}");
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

        public void PrintAllUsers()
        {
            string getUsersDataQuery = "SELECT id, name, surname, email FROM users";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, Connection);
            Connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}");
            }
            Connection.Close();
        }
        
        public void PrintAllWorkers()
        {
            string getUsersDataQuery = "SELECT id, name, surname, email, salary FROM workers";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, Connection);
            Connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t{reader[3]}\t{reader[4]}");
            }
            Connection.Close();
        }
    }
}