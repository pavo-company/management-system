using System;
using System.Data.SQLite;

namespace management_system
{
    public class Worker : Person
    {
        private static int _instanceId;
        private int Salary { get; init; }
         
        public Worker(string name, string surname, string email, int salary)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Salary = salary;
            Id = _instanceId;
            _instanceId++;
        }
        
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}Surname: {Surname}\tEmail: {Email}\tSalary: {Salary}";
        }
        
        public void AddToDatabase(Database db)
        {
            string query = "INSERT INTO workers ('id', 'name', 'surname', 'salary', 'email') VALUES (@id, @name, @surname, @salary, @email)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            db.Connection.Open();
            
            command.Parameters.AddWithValue("@id", Id);
            command.Parameters.AddWithValue("@name", Name);
            command.Parameters.AddWithValue("@surname", Surname);
            command.Parameters.AddWithValue("@salary", Salary);
            command.Parameters.AddWithValue("@email", Email);
            
            command.ExecuteNonQuery();
            db.Connection.Close();
        }
    }
}