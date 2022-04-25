using System;
using System.Data.SQLite;

namespace management_system
{
    public class Order
    {
        private int SupplierId { get; set; }
        private int ItemId { get; set; }
        private int Amount { get; set; }
        private string Date { get; set; }

        public Order(int supplierId, int itemId, int amount)
        {
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = DateTime.Today.ToString();
        }
        
        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO orders ('supplier_id', 'item_id', 'amount') VALUES (@supplier_id, @item_id, @amount)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@supplier_id", SupplierId);
            backupCommand.Parameters.AddWithValue("@supplier_id", SupplierId);
            
            command.Parameters.AddWithValue("@item_id", ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", ItemId);
            
            command.Parameters.AddWithValue("@amount", Amount);
            backupCommand.Parameters.AddWithValue("@amount", Amount);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}