using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using migration;

namespace migrations
{
    public class Migrations
    {
        private readonly SQLiteConnection _connection;
        private readonly  SQLiteConnection _backupConnection;
        
        
        public Migrations(SQLiteConnection connection, SQLiteConnection backupConnection)
        { 
            _connection = connection;
            _backupConnection = backupConnection;
            
        }

        public void MigrateAll()
        {
            _connection.Open();
            _backupConnection.Open();
            try
            {
                var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.Namespace != null && t.Namespace.StartsWith("migration.version"));
                
                foreach (var t in types)
                {
                    #nullable enable
                    Migration? instance = (Migration?) Activator.CreateInstance(t);

                    string getMigrationVersionQuery =
                        $"SELECT * FROM migrations WHERE version LIKE '{t.Name}';";
                    
                    SQLiteCommand command = new SQLiteCommand(getMigrationVersionQuery, _connection);
                    SQLiteCommand commandBackup = new SQLiteCommand(getMigrationVersionQuery, _backupConnection);
                    
                    SQLiteDataReader reader = command.ExecuteReader();
                    if(!reader.HasRows)
                    {
                        instance?.Up(_connection);
                        instance?.AddToDatabase(_connection);
                    }
                    reader = commandBackup.ExecuteReader();
                    if(!reader.HasRows)
                    {
                        instance?.Up(_backupConnection);
                        instance?.AddToDatabase(_backupConnection);
                    }
                }
            }
            catch
            {
                Console.WriteLine("An error has occurred in the MigrateAll() function");
            }
            _connection.Close();
            _backupConnection.Close();
        }
        
        public void Migrate(string version)
        {
            _connection.Open();
            _backupConnection.Open();
            
            var type = Assembly
                .GetExecutingAssembly()
                .GetType($"migration.version.{version}");
            
            #nullable enable
            Migration? instance = (Migration?) Activator.CreateInstance(type ?? throw new ArgumentException($"Version {version} not exists."));

            string getMigrationVersionQuery =
                $"SELECT * FROM migrations WHERE version LIKE '{type.Name}';";

            SQLiteCommand command = new SQLiteCommand(getMigrationVersionQuery, _connection);
            SQLiteCommand commandBackup = new SQLiteCommand(getMigrationVersionQuery, _backupConnection);


            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                instance?.Up(_connection);
                instance?.AddToDatabase(_connection);
            }

            reader = commandBackup.ExecuteReader();
            if (!reader.HasRows)
            {
                instance?.Up(_backupConnection);
                instance?.AddToDatabase(_backupConnection);
            }

            _connection.Close();
            _backupConnection.Close();
        }
    }
}