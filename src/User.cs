using System;
using System.Data.SQLite;

namespace management_system
{
    public class User : Person
    {
         
        public User(string name, string surname, string email)
        {
            Name = name;
            Email = email;
            Surname = surname;
        }
        public override string ToString()
        {
            return $"User:\n\tName: {Name}\n\tEmail: {Email}\n";
        }

        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO users ('name', 'surname', 'email') VALUES (@name, @surname, @email)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@surname", Surname);
            backupCommand.Parameters.AddWithValue("@surname", Surname);
            
            command.Parameters.AddWithValue("@email", Email);
            backupCommand.Parameters.AddWithValue("@email", Email);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}