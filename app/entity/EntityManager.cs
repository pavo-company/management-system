using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace management_system.app.entity
{
    public class EntityManager
    {
        private static Database _database;

        public EntityManager(Database database)
        {
            _database = database; 
        }

        /// <param name="id">User id</param>
        /// <returns>If the user exists it returns his object; otherwise null</returns>
        public User GetUser(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM users WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if(!reader.Read())
                return null;

            return new User(Convert.ToInt32(reader[0]),$"{reader[1]}", $"{reader[2]}", $"{reader[3]}");
        }

        /// <param name="id">Worker id</param>
        /// <returns>If the worker exists it returns his object; otherwise null</returns>
        public Worker GetWorker(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM workers WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Worker(Convert.ToInt32(reader[0]), $"{reader[1]}", $"{reader[2]}", $"{reader[4]}", Convert.ToInt32(reader[3]));
        }

        /// <param name="id">Supplier id</param>
        /// <returns>If the supplier exists it returns his object; otherwise null</returns>
        public Supplier GetSupplier(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM suppliers WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Supplier(Convert.ToInt32(reader[0]), $"{reader[1]}", $"{reader[2]}");
        }


        /// <param name="id">Order id</param>
        /// <returns>If the order exists it returns his object; otherwise null</returns>
        public Order GetOrder(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM orders WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Order(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToDateTime(reader[4]), Convert.ToBoolean(reader[5]));
        }

        /// <param name="id">Extraction id</param>
        /// <returns>If the extraction exists it returns his object; otherwise null</returns>
        public Extraction GetExtraction(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM extractions WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Extraction(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToInt32(reader[4]));
        }

        /// <param name="id">Tag id</param>
        /// <returns>If the tag exists it returns his object; otherwise null</returns>
        public Tag GetTag(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM tags WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Tag(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2]));
        }

        /// <param name="id">Item id</param>
        /// <returns>If the item exists and has tags, returns the list of its tags; otherwise null</returns>
        public List<Tag> GetItemTags(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM tags WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();
            List<Tag> tags = new List<Tag>();

            while (reader.Read())
                tags.Add(new Tag(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2])));

            if (tags.Count == 0) 
                return null;

            return tags;
        }
        /// <param name="item">Item object</param>
        /// <returns>If the item exists and has tags, returns the list of its tags; otherwise null</returns>
        public List<Tag> GetItemTags(Item item) => GetItemTags(item.Id);


        /// <param name="id">Item id</param>
        /// <returns>If the item exists it returns his object; otherwise null</returns>
        public Item GetItem(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM items WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Item(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToInt32(reader[4]), GetItemTags(Convert.ToInt32(reader[0])));
        }

    }
}
