using System;
using System.Data.SQLite;

namespace management_system
{
    public class Extraction
    {
        private int WorkerId { get; set; }
        private int ItemId { get; set; }
        private int UserId { get; set; }
        private int Amount { get; set; }

        public Extraction(int workerId, int itemId, int userId, int amount)
        {
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