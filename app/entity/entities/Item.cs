using management_system.app.entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace management_system
{
    public class Item : Entity
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Amount { get; set; }
        public int MinAmount { get; set; }
        public int Price { get; set; }
        public List<Tag> Tags { get; set; }

        public int GetId() => Id;
        public string DatabaseTableName() => "items";
        public string[] DatabaseColumnNames() => new string[] { "name", "amount", "min_amount", "price" };
        public string[] DatabaseColumnValues() => new string[] { $"'{Name}'", $"{Amount}", $"{MinAmount}", $"{Price}" };

        public Item(string name, int amount, int minAmount, int price, List<Tag> tags)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            MinAmount = minAmount;
            Price = price;
            Tags = tags;
        }

        public Item(string name, int amount, int minAmount, int price)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            MinAmount = minAmount;
            Price = price;
            Tags = new List<Tag>();
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