using System;
using System.Data.SQLite;

namespace management_system
{
    public class Order
    {
        public int Id { get; init; }
        public int SupplierId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsCyclic { get; set; }

        public Order(int supplierId, int itemId, int amount, bool isCyclic)
        {
            Id = -1;
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = DateTime.Today;
            IsCyclic = isCyclic;
        }
        public Order(int id, int supplierId, int itemId, int amount, DateTime date, bool isCyclic)
        {
            Id = id;
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = date;
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