using System;
using System.Data.SQLite;

namespace management_system
{
    public class Order
    {
        private int SupplierId { get; set; }
        private int ItemId { get; set; }
        private int Amount { get; set; }
        private DateTime Date { get; set; }
        private bool IsCyclic { get; set; }

        public Order(int supplierId, int itemId, int amount, bool isCyclic)
        {
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = DateTime.Today;
            IsCyclic = isCyclic;
        }
        
        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO orders ('supplier_id', 'item_id', 'amount', 'date', 'is_cyclic') VALUES (@supplier_id, @item_id, @amount, @date, @is_cyclic)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@supplier_id", SupplierId);
            backupCommand.Parameters.AddWithValue("@supplier_id", SupplierId);
            
            command.Parameters.AddWithValue("@item_id", ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", ItemId);
            
            command.Parameters.AddWithValue("@amount", Amount);
            backupCommand.Parameters.AddWithValue("@amount", Amount);
            
            command.Parameters.AddWithValue("@date", Date.ToString());
            backupCommand.Parameters.AddWithValue("@date", Date.ToString());
            
            command.Parameters.AddWithValue("@is_cyclic", IsCyclic);
            backupCommand.Parameters.AddWithValue("@is_cyclic", IsCyclic);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }

        public void UpdateDatabaseAfterDate(Database db)
        {
            string getAmount = $"SELECT amount FROM items WHERE id LIKE '{ItemId}';";
            SQLiteCommand command = new SQLiteCommand(getAmount, db.Connection);

            db.Open();

            int amount = Convert.ToInt32(command.ExecuteScalar());
            string updateQuery = $"UPDATE items SET amount = {Amount + amount} WHERE id LIKE '{ItemId}';";
            
            SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, db.Connection);
            SQLiteCommand updateBackupCommand = new SQLiteCommand(updateQuery, db.BackupConnection);

            updateCommand.ExecuteNonQuery();
            updateBackupCommand.ExecuteNonQuery();

            if (IsCyclic)
            {
                Date = Date.AddMonths(1);
                string updateMonthQuery = $"UPDATE orders SET date = {Date.ToString()} WHERE id LIKE '{ItemId}'";
                
                SQLiteCommand updateMonth = new SQLiteCommand(updateMonthQuery, db.Connection);
                SQLiteCommand updateBackupMonth = new SQLiteCommand(updateMonthQuery, db.BackupConnection);

                updateMonth.ExecuteNonQuery();
                updateBackupMonth.ExecuteNonQuery();
            }
            
            db.Close();
        }
    }
}