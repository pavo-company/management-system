using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace management_system.app.entity
{
    public class EntityManager
    {
        private static Database _database;
        private static List<Action> _update;

        public EntityManager(Database database)
        {
            _database = database; 
            _update = new List<Action>();
        }


        /// <summary>
        /// Update {columns} in {tableName}
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        private bool UpdateTable(string tableName, string[] columns, string[] args, int id)
        {
            string col = "";

            if (columns.Length != args.Length)
                return false;

            for (int i = 0; i < columns.Length; i++)
                col += $"{columns[i]} = {args[i]}, ";

            string updateQuery = $"UPDATE {tableName} SET {col.Substring(0, col.Length - 2)} WHERE id = {id};";

            SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, _database.Connection);
            SQLiteCommand updateBackupCommand = new SQLiteCommand(updateQuery, _database.BackupConnection);

            int res = updateCommand.ExecuteNonQuery();
            res += updateBackupCommand.ExecuteNonQuery();
            return res != 0;
        }

        /// <summary>
        /// Check if an entity has changed.
        /// If so, it updates it in the database.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        private void CheckEntityUpdate(string[] values, Func<string[]> getCurrValues, Action update)
        {
            string[] currValues = getCurrValues();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != currValues[i])
                {
                    update();
                    break;
                }
            }
        }

        private bool InsertInto(Entity em)
        {
            string col = "";
            string val = "";
            for (int i = 0; i < em.DatabaseColumnNames().Length; i++)
            {
                col += $"'{em.DatabaseColumnNames()[i]}', ";
                val += $"{em.DatabaseColumnValues()[i]}, ";
            }
            string query = $"INSERT INTO {em.DatabaseTableName()} ({col.Substring(0, col.Length - 2)}) VALUES ({val.Substring(0, val.Length - 2)})";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }

        /// <summary>
        /// Updates the database of changed entities that were retrieved by EntityManager or added manually.
        /// When finished, clears the list and doesn't see the new changes (you have to add the entities again manually).
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool flush()
        {
            foreach (var action in _update)
                action();

            _update.Clear();
            return true;
        }

        /// <summary>
        /// Updates the database of changed entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool Update(Entity em) => UpdateTable(em.DatabaseTableName(), em.DatabaseColumnNames(), em.DatabaseColumnValues(), em.GetId());

        /// <summary>
        /// Add entity to database if not exists; otherwise adds entities to observed entities
        /// delays updates/adds until flush() is used
        /// </summary>
        public void Add(Entity em)
        {
            if (em.GetId() == -1)
                _update.Add(() => InsertInto(em));
            else
                _update.Add(() => CheckEntityUpdate(em.DatabaseColumnNames(),
                                                        () => em.DatabaseColumnValues(),
                                                        () => Update(em)));
        }

        /// <returns>Returns table of entities in string </returns>
        public List<string> GetAllEntities(string table)
        {
            string getUsersDataQuery = $"SELECT * FROM {table}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();
            List<string> entities = new List<string>();
            while (reader.Read())
            {
                string @string = "";
                foreach (var el in reader.GetValues())
                {
                    @string += reader[el.ToString()] + " ";
                }
                entities.Add(@string);
            }
            return entities;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>If the user exists it returns his object; otherwise null</returns>
        public User GetUser(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM users WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if(!reader.Read())
                return null;

            User user = new User(Convert.ToInt32(reader[0]), $"{reader[1]}", $"{reader[2]}", $"{reader[3]}");
            Add(user);
            return user;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Worker id</param>
        /// <returns>If the worker exists it returns his object; otherwise null</returns>
        public Worker GetWorker(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM workers WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Worker worker = new Worker(Convert.ToInt32(reader[0]), $"{reader[1]}", $"{reader[2]}", $"{reader[4]}", Convert.ToInt32(reader[3]));
            Add(worker);
            return worker;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Supplier id</param>
        /// <returns>If the supplier exists it returns his object; otherwise null</returns>
        public Supplier GetSupplier(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM suppliers WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Supplier supplier = new Supplier(Convert.ToInt32(reader[0]), $"{reader[1]}", $"{reader[2]}");
            Add(supplier);
            return supplier;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>If the order exists it returns his object; otherwise null</returns>
        public Order GetOrder(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM orders WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Order order = new Order(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToDateTime(reader[4]), Convert.ToBoolean(reader[5]));
            Add(order);
            return order;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Extraction id</param>
        /// <returns>If the extraction exists it returns his object; otherwise null</returns>
        public Extraction GetExtraction(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM extractions WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Extraction extraction = new Extraction(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToInt32(reader[4]));
            Add(extraction);
            return extraction;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <returns>If the tag exists it returns his object; otherwise null</returns>
        public Tag GetTag(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM tags WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Tag tag = new Tag(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2]));
            Add(tag);
            return tag;
        }

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>If the item exists and has tags, returns the list of its tags; otherwise null</returns>
        public List<Tag> GetItemTags(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM tags WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();
            List<Tag> tags = new List<Tag>();

            while (reader.Read())
            {
                Tag tag = new Tag(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2]));
                Add(tag);
                tags.Add(tag);
            }

            if (tags.Count == 0) 
                return null;

            return tags;
        }
        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="item">Item object</param>
        /// <returns>If the item exists and has tags, returns the list of its tags; otherwise null</returns>
        public List<Tag> GetItemTags(Item item) => GetItemTags(item.Id);

        ///<summary>
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>If the item exists it returns his object; otherwise null</returns>
        public Item GetItem(int id)
        {
            string getUsersDataQuery = $"SELECT * FROM items WHERE id = {id}";
            SQLiteCommand command = new SQLiteCommand(getUsersDataQuery, _database.Connection);

            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            Item item = new Item(Convert.ToInt32(reader[0]), $"{reader[1]}", Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]), Convert.ToInt32(reader[4]), GetItemTags(Convert.ToInt32(reader[0])));

            Add(item);
            return item;
        }
    }
}
