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
            {
                col += $"{columns[i]} = {args[i]}, ";
            }

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

        /// <summary>
        /// Updates the database of changed entities that were retrieved by EntityManager or added manually.
        /// When finished, clears the list and doesn't see the new changes (you have to add the entities again manually).
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool flush()
        {
            foreach(var action in _update)
                action();

            _update.Clear();
            return true;
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
            _update.Add(() => CheckEntityUpdate(new string[] {$"{reader[1]}", $"{reader[2]}", $"{reader[3]}" },
                                                    () => user.DatabaseColumnValues(),
                                                    () => UpdateUser(user)));
            return user;
        }
        /// <summary>
        /// Add user to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>If adding user went well true; otherwise false</returns>
        public bool AddUser(User user)
        {
            if(GetUser(user.Id) != null)
                return false;

            string query =
                "INSERT INTO users ('name', 'surname', 'tin') VALUES (@name, @surname, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@name", user.Name);
            backupCommand.Parameters.AddWithValue("@name", user.Name);

            command.Parameters.AddWithValue("@surname", user.Surname);
            backupCommand.Parameters.AddWithValue("@surname", user.Surname);

            command.Parameters.AddWithValue("@tin", user.Tin);
            backupCommand.Parameters.AddWithValue("@tin", user.Tin);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }
        /// <summary>
        /// Updates the database of changed user entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateUser(User user) => UpdateTable("users",
                                                    user.DatabaseColumnNames(),
                                                    user.DatabaseColumnValues(),
                                                    user.Id);

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
            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}", $"{reader[4]}", $"{reader[3]}" },
                                                    () => worker.DatabaseColumnValues(),
                                                    () => UpdateWorker(worker)));
            return worker;
        }
        /// <summary>
        /// Add worker to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="worker">Worker object</param>
        /// <returns>If adding worker went well true; otherwise false</returns>
        public bool AddWorker(Worker worker)
        {
            if (GetWorker(worker.Id) != null)
                return false;

            string query =
                "INSERT INTO workers ('name', 'surname', 'salary', 'tin') VALUES (@name, @surname, @salary, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@name", worker.Name);
            backupCommand.Parameters.AddWithValue("@name", worker.Name);

            command.Parameters.AddWithValue("@surname", worker.Surname);
            backupCommand.Parameters.AddWithValue("@surname", worker.Surname);

            command.Parameters.AddWithValue("@salary", worker.Salary);
            backupCommand.Parameters.AddWithValue("@salary", worker.Salary);

            command.Parameters.AddWithValue("@tin", worker.Tin);
            backupCommand.Parameters.AddWithValue("@tin", worker.Tin);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }
        /// <summary>
        /// Updates the database of changed worker entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateWorker(Worker worker) => UpdateTable("workers",
                                                           worker.DatabaseColumnNames(),
                                                           worker.DatabaseColumnValues(),
                                                           worker.Id);

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
            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}"},
                                                    () => supplier.DatabaseColumnValues(),
                                                    () => UpdateSupplier(supplier)));
            return supplier;
        }
        /// <summary>
        /// Add supplier to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="supplier">Supplier object</param>
        /// <returns>If adding supplier went well true; otherwise false</returns>
        public bool AddSupplier(Supplier supplier)
        {
            if (GetSupplier(supplier.Id) != null)
                return false;

            string query =
                "INSERT INTO suppliers ('name', 'tin') VALUES (@name, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@name", supplier.Name);
            backupCommand.Parameters.AddWithValue("@name", supplier.Name);

            command.Parameters.AddWithValue("@tin", supplier.Tin);
            backupCommand.Parameters.AddWithValue("@tin", supplier.Tin);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();

            return true;
        }
        /// <summary>
        /// Updates the database of changed supplier entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateSupplier(Supplier supplier) => UpdateTable("suppliers",
                                                                supplier.DatabaseColumnNames(),
                                                                supplier.DatabaseColumnValues(),
                                                                supplier.Id);

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
            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}", $"{reader[3]}", $"{reader[4]}", $"{reader[5]}" },
                                                    () => order.DatabaseColumnValues(),
                                                    () => UpdateOrder(order)));
            return order;
        }
        /// <summary>
        /// Add order to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns>If adding order went well true; otherwise false</returns>
        public bool AddOrder(Order order)
        {
            if (GetOrder(order.Id) != null)
                return false;

            string query =
                "INSERT INTO orders ('supplier_id', 'item_id', 'amount', 'date', 'is_cyclic') VALUES (@supplier_id, @item_id, @amount, @date, @is_cyclic)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@supplier_id", order.SupplierId);
            backupCommand.Parameters.AddWithValue("@supplier_id", order.SupplierId);

            command.Parameters.AddWithValue("@item_id", order.ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", order.ItemId);

            command.Parameters.AddWithValue("@amount", order.Amount);
            backupCommand.Parameters.AddWithValue("@amount", order.Amount);

            command.Parameters.AddWithValue("@date", order.Date.ToString());
            backupCommand.Parameters.AddWithValue("@date", order.Date.ToString());

            command.Parameters.AddWithValue("@is_cyclic", order.IsCyclic);
            backupCommand.Parameters.AddWithValue("@is_cyclic", order.IsCyclic);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }
        /// <summary>
        /// Updates the database of changed order entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateOrder(Order order) => UpdateTable("orders",
                                                        order.DatabaseColumnNames(),
                                                        order.DatabaseColumnValues(),
                                                        order.Id);

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
            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}", $"{reader[3]}", $"{reader[4]}"},
                                                    () => extraction.DatabaseColumnValues(),
                                                    () => UpdateExtraction(extraction)));
            return extraction;
        }
        /// <summary>
        /// Add extraction to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="extraction">Extraction object</param>
        /// <returns>If adding extraction went well true; otherwise false</returns>
        public bool AddExtraction(Extraction extraction)
        {
            if (GetExtraction(extraction.Id) != null)
                return false;

            string query =
                "INSERT INTO extractions ('worker_id', 'item_id', 'amount', 'user_id') VALUES (@worker_id, @item_id, @amount, @user_id)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@worker_id", extraction.WorkerId);
            backupCommand.Parameters.AddWithValue("@worker_id", extraction.WorkerId);

            command.Parameters.AddWithValue("@item_id", extraction.ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", extraction.ItemId);

            command.Parameters.AddWithValue("@amount", extraction.Amount);
            backupCommand.Parameters.AddWithValue("@amount", extraction.Amount);

            command.Parameters.AddWithValue("@user_id", extraction.UserId);
            backupCommand.Parameters.AddWithValue("@user_id", extraction.UserId);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }
        /// <summary>
        /// Updates the database of changed extraction entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateExtraction(Extraction extraction) => UpdateTable("extractions",
                                                                       extraction.DatabaseColumnNames(),
                                                                       extraction.DatabaseColumnValues(),
                                                                       extraction.Id);

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
            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}"},
                                                    () => tag.DatabaseColumnValues(),
                                                    () => UpdateTag(tag)));
            return tag;
        }
        /// <summary>
        /// Add tag to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="tag">Tag object</param>
        /// <returns>If adding tag went well true; otherwise false</returns>
        public bool AddTag(Tag tag)
        {
            if (GetTag(tag.Id) != null)
                return false;

            string query =
                "INSERT INTO tags ('name', 'item_id') VALUES (@name, @item_id)";

            SQLiteCommand command = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, _database.BackupConnection);


            command.Parameters.AddWithValue("@name", tag.Name);
            backupCommand.Parameters.AddWithValue("@name", tag.Name);

            command.Parameters.AddWithValue("@item_id", tag.ItemId);
            backupCommand.Parameters.AddWithValue("@item_id", tag.ItemId);

            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();


            return true;
        }
        /// <summary>
        /// Updates the database of changed tag entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateTag(Tag tag) => UpdateTable("tags",
                                                  tag.DatabaseColumnNames(),
                                                  tag.DatabaseColumnValues(),
                                                  tag.Id);

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
                _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}" },
                                                        () => tag.DatabaseColumnValues(),
                                                        () => UpdateTag(tag)));
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

            _update.Add(() => CheckEntityUpdate(new string[] { $"{reader[1]}", $"{reader[2]}", $"{reader[2]}", $"{reader[3]}", $"{reader[4]}" },
                                                    () => item.DatabaseColumnValues(),
                                                    () => UpdateItem(item)));
            return item;
        }
        /// <summary>
        /// Add item to database
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <param name="item">Item object</param>
        /// <returns>If adding item went well true; otherwise false</returns>
        public bool AddItem(Item item)
        {

            string query =
                "INSERT INTO items ('name', 'amount', 'min_amount', 'price') VALUES (@name, @amount, @min, @price)";

            SQLiteCommand addCommand = new SQLiteCommand(query, _database.Connection);
            SQLiteCommand addBackupCommand = new SQLiteCommand(query, _database.BackupConnection);


            if (GetItem(item.Id) != null)
                return false;

            addCommand.Parameters.AddWithValue("@name", item.Name);
            addBackupCommand.Parameters.AddWithValue("@name", item.Name);

            addCommand.Parameters.AddWithValue("@amount", item.Amount);
            addBackupCommand.Parameters.AddWithValue("@amount", item.Amount);

            addCommand.Parameters.AddWithValue("@min", item.MinAmount);
            addBackupCommand.Parameters.AddWithValue("@min", item.MinAmount);

            addCommand.Parameters.AddWithValue("@price", item.Price);
            addBackupCommand.Parameters.AddWithValue("@price", item.Price);

            addCommand.ExecuteNonQuery();
            addBackupCommand.ExecuteNonQuery();


            foreach (Tag tag in item.Tags)
                AddTag(tag);

            return true;
        }
        /// <summary>
        /// Updates the database of changed item entity.
        /// [THE CONNECTION TO THE DATABASE MUST BE OPEN]
        /// </summary>
        /// <returns>If everything went well true; otherwise false</returns>
        public bool UpdateItem(Item item) => UpdateTable("items",
                                                     item.DatabaseColumnNames(),
                                                     item.DatabaseColumnValues(),
                                                     item.Id);
    }
}
