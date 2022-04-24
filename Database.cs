using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace management_system
{
    public class Database
    {
        private const string DatabasePath = "../../../database.sqlite";
        private const string BackupDatabasePath = "../../../backup.sqlite";
        public SQLiteConnection Connection;
        public SQLiteConnection BackupConnection;

        public Database()
        {
            CreateDatabaseIfNotExists();
            
            string getTablesQuery =
                "SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
            Connection = new SQLiteConnection($"Data Source={DatabasePath}");
            BackupConnection = new SQLiteConnection($"Data Source={BackupDatabasePath}");

            List<string> tables = new List<string>();
            SQLiteCommand getTables = new SQLiteCommand(getTablesQuery, Connection);
            
            Open();
            
            SQLiteDataReader reader = getTables.ExecuteReader();
            while (reader.Read())
                tables.Add(Convert.ToString(reader["name"]));
                
            CreateTablesIfNotExists(tables, Connection);
            CreateTablesIfNotExists(tables, BackupConnection);
            
            Close();
        }

        private void CreateTablesIfNotExists(List<string> tables, SQLiteConnection connection)
        {
            string createUserTable = 
                "create table users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, surname TEXT, email TEXT);";
            string createWorkersTable =
                "create table workers (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, surname TEXT, salary INTEGER, email TEXT);";
            string createItemsTable = 
                "create table items (id INTEGER PRIMARY KEY AUTOINCREMENT,  name TEXT, amount INTEGER, min_amount INTEGER);";
            if (!tables.Contains("users"))
            {
                SQLiteCommand addUserTab = new SQLiteCommand(createUserTable, connection);
                addUserTab.ExecuteNonQuery();
            }

            if (!tables.Contains("workers"))
            {
                SQLiteCommand addWorkersTab = new SQLiteCommand(createWorkersTable, connection);
                addWorkersTab.ExecuteNonQuery();
            }

            if (!tables.Contains("items"))
            {
                SQLiteCommand addItemsTab = new SQLiteCommand(createItemsTable, connection);
                addItemsTab.ExecuteNonQuery();
            }
        }

        private static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(DatabasePath))
                SQLiteConnection.CreateFile(DatabasePath);
            
            if (!File.Exists(BackupDatabasePath))
                SQLiteConnection.CreateFile(BackupDatabasePath);
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

        public int HowManyRecords(string table)
        {   
            string query = $"SELECT COUNT(id) FROM {table}";
            SQLiteCommand command = new SQLiteCommand(query, Connection);
            Connection.Open();
            int counter = Convert.ToInt32(command.ExecuteScalar());;
            Connection.Close();
            return counter;
        }

        public void Search(string table, string[] columns, string template, string templateColumn)
        {
            string query = 
                $"SELECT {String.Join(",", columns)} FROM {table} WHERE {templateColumn} LIKE '%{template}%'";
            SQLiteCommand command = new SQLiteCommand(query, Connection);
            
            Connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
               for(int column = 0; column < reader.FieldCount; column++)
               {
                   Console.Write(reader[column]);
                   if(column+1 < reader.FieldCount)
                       Console.Write("\t");
                   else
                       Console.WriteLine();
               }
            }
            Connection.Close();
        }

        public void Open()
        {
            Connection.Open();
            BackupConnection.Open();
        }

        public void Close()
        {
            Connection.Close();
            BackupConnection.Close();
        }
    }
}