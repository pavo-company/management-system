using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace management_system
{
    public class Item
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Amount { get; set; }
        public int MinAmount { get; set; }
        public int Price { get; set; }
        public List<Tag> Tags { get; set; }

        public Item(string name, int amount, int minAmount, int price, List<Tag> tags)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            MinAmount = minAmount;
            Price = price;
            Tags = tags;
        }

        public Item(int id, string name, int amount, int minAmount, int price, List<Tag> tags)
        {
            Id = id;
            Name = name;
            Amount = amount;
            MinAmount = minAmount;
            Price = price;
            Tags = tags;
        }

        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO items ('name', 'amount', 'min_amount', 'price') VALUES (@name, @amount, @min, @price)";
            
            SQLiteCommand addCommand = new SQLiteCommand(query, db.Connection);
            SQLiteCommand addBackupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();

            addCommand.Parameters.AddWithValue("@name", Name);
            addBackupCommand.Parameters.AddWithValue("@name", Name);
            
            addCommand.Parameters.AddWithValue("@amount", Amount);
            addBackupCommand.Parameters.AddWithValue("@amount", Amount);
            
            addCommand.Parameters.AddWithValue("@min", MinAmount);
            addBackupCommand.Parameters.AddWithValue("@min", MinAmount);

            addCommand.Parameters.AddWithValue("@price", Price);
            addBackupCommand.Parameters.AddWithValue("@price", Price);

            addCommand.ExecuteNonQuery();
            addBackupCommand.ExecuteNonQuery();
            
            db.Close();

            foreach (Tag tag in Tags)
            {
                tag.AddToDatabase(db);
            }
        }

        public void ReduceItem(string name, int amount, Database db)
        {
            string query = $"SELECT amount FROM items WHERE name={name}";
            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            
            db.Connection.Open();
            amount -= Convert.ToInt32(command.ExecuteScalar());
            
            if (amount < 0)
                throw new Exception("You haven't that amount of this item in your database");
            if (amount <= MinAmount)
                Console.WriteLine("Amount of this product reached his minimum amount");
            
            string updateQuery = $"UPDATE items SET name={amount} WHERE name={name}";
            SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, db.Connection);
            
            updateCommand.ExecuteNonQuery();
            db.Connection.Close();
        }
        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }
        public void RemoveTag(Tag tag)
        {
            Tags.Remove(tag);
        }
    }
}