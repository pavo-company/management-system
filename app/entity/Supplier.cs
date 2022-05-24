using System.Data.SQLite;

namespace management_system
{
    public class Supplier : Person
    {
        public Supplier(string name, string tin)
        {
            Name = name;
            Tin = tin;
        }
        
        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO suppliers ('name', 'tin') VALUES (@name, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@tin", Tin);
            backupCommand.Parameters.AddWithValue("@tin", Tin);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}