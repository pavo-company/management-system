using System;
using System.Data.SQLite;

namespace management_system
{
    public class Extraction
    {
        public int Id { get; init; }
        public int WorkerId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }

        public Extraction(int workerId, int itemId, int userId, int amount)
        {
            Id = -1;
            WorkerId = workerId;
            ItemId = itemId;
            UserId = userId;
            Amount = amount;
        }

        public Extraction(int id, int workerId, int itemId, int userId, int amount)
        {
            Id = id;
            WorkerId = workerId;
            ItemId = itemId;
            UserId = userId;
            Amount = amount;
        }

        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO extractions ('worker_id', 'item_id', 'amount', 'user_id') VALUES (@worker_id, @item_id, @amount, @user_id)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@worker_id", WorkerId);
            backupCommand.Parameters.AddWithValue("@worker_id", WorkerId);
            
            command.Parameters.AddWithValue("@item_id", ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", ItemId);
            
            command.Parameters.AddWithValue("@amount", Amount);
            backupCommand.Parameters.AddWithValue("@amount", Amount);

            command.Parameters.AddWithValue("@user_id", UserId);
            backupCommand.Parameters.AddWithValue("@user_id", UserId);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}