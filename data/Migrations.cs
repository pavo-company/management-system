using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
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
                SQLiteCommand commandBackup = new SQLiteCommand(getMigrationVersionQuery, Connection);
                
                
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
        }

    }

}