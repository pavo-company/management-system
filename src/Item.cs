using System;
using System.Data.SQLite;

namespace management_system
{
    public class Item
    {
        private static int _instanceId;
        private int Id { get; init; }
        private string Name { get; init; }
        private int Amount { get; set; }

        private int MinAmount { get; set; }

        public Item(string name, int amount, int minAmount)
        {
            Id = _instanceId;
            Name = name;
            Amount = amount;
            MinAmount = minAmount;
            _instanceId++;
        }

        public void AddToDatabase(Database db)
        {
            string query = "INSERT INTO items ('id', 'name', 'amount', 'min_amount') VALUES (@id, @name, @amount, @min)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            db.Connection.Open();
            command.Parameters.AddWithValue("@id", Id);
            command.Parameters.AddWithValue("@name", Name);
            command.Parameters.AddWithValue("@amount", Amount);
            command.Parameters.AddWithValue("@min", MinAmount);
            var result = command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine(result);
        }
    }
}