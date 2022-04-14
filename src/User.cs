using System;
using System.Data.SQLite;

namespace management_system
{
    public class User : Person
    {
        
        private static int _instanceId;
         
        public User(string name, string surname, string email)
        {
            Name = name;
            Email = email;
            Surname = surname;
            Id = _instanceId;
            _instanceId++;
        }
        public override string ToString()
        {
            return $"User:\n\tName: {Name}\n\tEmail: {Email}\n\tId: {Id}";
        }

        public void AddToDatabase(Database db)
        {
            string query = $"INSERT INTO users ('name', 'surname', 'email') VALUES (@name, @surname, @email)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            db.Connection.Open();
            command.Parameters.AddWithValue("@name", Name);
            command.Parameters.AddWithValue("@surname", Surname);
            command.Parameters.AddWithValue("@email", Email);
            var result = command.ExecuteNonQuery();
            db.Connection.Close();
            Console.WriteLine(result);
        }
    }
}