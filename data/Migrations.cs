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
        public SQLiteConnection Connection;
        public SQLiteConnection BackupConnection;
        private List<string> tables;
        
        
        public Migrations(SQLiteConnection Connection, SQLiteConnection BackupConnection)
        { 
            this.Connection = Connection;
            this.BackupConnection = BackupConnection;
            
        }

        public void MigrateAll()
        {
            Connection.Open();
            BackupConnection.Open();
            
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("migration.version"));
            
            foreach (var t in types)
            {
                Migration instance = (Migration)Activator.CreateInstance(t);

                string getMigrationVersionQuery =
                    $"SELECT * FROM migrations WHERE version LIKE '{Convert.ToString(t.Name)}';";
                
                SQLiteCommand command = new SQLiteCommand(getMigrationVersionQuery, Connection);
                SQLiteCommand commandBackup = new SQLiteCommand(getMigrationVersionQuery, BackupConnection);
                
                SQLiteDataReader reader = command.ExecuteReader();
                if(!reader.HasRows)
                {
                    instance.Up(Connection);
                    instance.AddToDatabase(Connection);
                }
                reader = commandBackup.ExecuteReader();
                if(!reader.HasRows)
                {
                    instance.Up(BackupConnection);
                    instance.AddToDatabase(BackupConnection);
                }
            }
            
            Connection.Close();
            BackupConnection.Close();
        }
        
        public void Migrate(string version)
        {
            Connection.Open();
            BackupConnection.Open();
            
            try
            {
                var type = Assembly
                    .GetExecutingAssembly()
                    .GetType($"migration.version.{version}");
                
                Migration instance = (Migration) Activator.CreateInstance(type);

                string getMigrationVersionQuery =
                    $"SELECT * FROM migrations WHERE version LIKE '{Convert.ToString(type.Name)}';";

                SQLiteCommand command = new SQLiteCommand(getMigrationVersionQuery, Connection);
                SQLiteCommand commandBackup = new SQLiteCommand(getMigrationVersionQuery, BackupConnection);


                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    instance.Up(Connection);
                    instance.AddToDatabase(Connection);
                }

                reader = commandBackup.ExecuteReader();
                if (!reader.HasRows)
                {
                    instance.Up(BackupConnection);
                    instance.AddToDatabase(BackupConnection);
                }
            }
            catch
            {
                Console.WriteLine($"Version {version} not exists.");
            }
            
            Connection.Close();
            BackupConnection.Close();
        }
    }
}