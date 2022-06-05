using System;
using System.Data.SQLite;

namespace management_system
{
    public class Tag
    {
        public int Id { get; }
        public string Name { get; set; }
        public int ItemId { get; set; }

        public Tag(string name, int itemId)
        {
            Id = -1;
            Name = name;
            ItemId = itemId;
        }

        public Tag(int id, string name, int itemId)
        {
            Id = id;
            Name = name;
            ItemId = itemId;
        }

        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO tags ('name', 'item_id') VALUES (@name, @item_id)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@item_id", ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", ItemId);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}