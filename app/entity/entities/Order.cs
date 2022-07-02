using management_system.app.entity;
using System;
using System.Data.SQLite;

namespace management_system
{
    public class Order : Entity
    {
        public int Id { get; init; }
        public int SupplierId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsCyclic { get; set; }
        public int GetId() => Id;
        public string DatabaseTableName() => "orders";
        public string[] DatabaseColumnNames() => new string[] { "supplier_id", "item_id", "amount", "date", "is_cyclic" };
        public string[] DatabaseColumnValues() => new string[] { $"{SupplierId}", $"{ItemId}", $"{Amount}", $"'{Date.ToString("MM/dd/yyyy")}'", $"{Convert.ToInt16(IsCyclic)}" };

        public Order(int supplierId, int itemId, int amount, bool isCyclic)
        {
            Id = -1;
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = DateTime.Today;
            IsCyclic = isCyclic;
        }
        public Order(int supplierId, int itemId, int amount, DateTime date, bool isCyclic)
        {
            Id = -1;
            SupplierId = supplierId;
            ItemId = itemId;
            Amount = amount;
            Date = date;
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