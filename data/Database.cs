using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace management_system
{
    public class Database
    {
        private const string DatabasePath = "../../../data/database.sqlite";
        private const string BackupDatabasePath = "../../../data/backup.sqlite";
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

        private static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(DatabasePath))
                SQLiteConnection.CreateFile(DatabasePath);
            
            if (!File.Exists(BackupDatabasePath))
                SQLiteConnection.CreateFile(BackupDatabasePath);
        }

        private void CreateTablesIfNotExists(List<string> tables, SQLiteConnection connection)
        {
            if (!tables.Contains("users"))
            {
                string createUserTable = 
                    "create table users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, surname TEXT, tin TEXT);";
                SQLiteCommand addUserTab = new SQLiteCommand(createUserTable, connection);
                addUserTab.ExecuteNonQuery();
            }

            if (!tables.Contains("workers"))
            {
                string createWorkersTable =
                    "create table workers (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, surname TEXT, salary INTEGER, tin TEXT);";
                SQLiteCommand addWorkersTab = new SQLiteCommand(createWorkersTable, connection);
                addWorkersTab.ExecuteNonQuery();
            }

            if (!tables.Contains("items"))
            {
                string createItemsTable = 
                    "create table items (id INTEGER PRIMARY KEY AUTOINCREMENT,  name TEXT, amount INTEGER, min_amount INTEGER);";
                SQLiteCommand addItemsTab = new SQLiteCommand(createItemsTable, connection);
                addItemsTab.ExecuteNonQuery();
            }
            
            if (!tables.Contains("tags"))
            {
                string createTagsTable = 
                    "create table tags (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, item_id INTEGER," + 
                    "FOREIGN KEY (item_id) REFERENCES items(id));";
                SQLiteCommand addTagsTab = new SQLiteCommand(createTagsTable, connection);
                addTagsTab.ExecuteNonQuery();
            }
            
            if (!tables.Contains("orders"))
            {
                string createOrdersTable = 
                    "create table orders (id INTEGER PRIMARY KEY AUTOINCREMENT,  supplier_id TEXT, item_id INTEGER, amount INTEGER, date TEXT, is_cyclic NUMERIC(1)," + 
                    "FOREIGN KEY (supplier_id) REFERENCES suppliers(id), FOREIGN KEY (item_id) REFERENCES items(id));";
                SQLiteCommand addOrdersTab = new SQLiteCommand(createOrdersTable, connection);
                addOrdersTab.ExecuteNonQuery();
            }
            
            if (!tables.Contains("suppliers"))
            {
                string createSuppliersTable = 
                    "create table suppliers (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, tin TEXT);";
                SQLiteCommand addSuppliersTab = new SQLiteCommand(createSuppliersTable, connection);
                addSuppliersTab.ExecuteNonQuery();
            }
            
            if (!tables.Contains("extractions"))
            {
                string createExtractionsTable = 
                    "create table extractions (id INTEGER PRIMARY KEY AUTOINCREMENT, worker_id INTEGER, item_id INTEGER, amount INTEGER, user_id INTEGER," +
                    "FOREIGN KEY (worker_id) REFERENCES workers(id), FOREIGN KEY (item_id) REFERENCES items(id), FOREIGN KEY (user_id) REFERENCES users(id));";
                SQLiteCommand addExtractionsTab = new SQLiteCommand(createExtractionsTable, connection);
                addExtractionsTab.ExecuteNonQuery();
            }
        }

        public void PrintAllUsers()
        {
            string getUsersDataQuery = "SELECT id, name, surname, tin FROM users";
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
            string getUsersDataQuery = "SELECT id, name, surname, tin, salary FROM workers";
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
            int counter;
            try
            {
                counter = Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                return 0;
            }
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