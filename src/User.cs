using System;
using System.Data.SQLite;

namespace management_system
{
    public class User : Person
    {
         
        public User(string name, string surname, string tin)
        {
            Name = name;
            Tin = tin;
            Surname = surname;
        }
        public override string ToString()
        {
            return $"User:\tName: {Name}\tTin: {Tin}";
        }

        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO users ('name', 'surname', 'tin') VALUES (@name, @surname, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@surname", Surname);
            backupCommand.Parameters.AddWithValue("@surname", Surname);
            
            command.Parameters.AddWithValue("@tin", Tin);
            backupCommand.Parameters.AddWithValue("@tin", Tin);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}