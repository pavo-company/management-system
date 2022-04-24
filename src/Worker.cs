using System;
using System.Data.SQLite;

namespace management_system
{
    public class Worker : Person
    {
        private int Salary { get; init; }
         
        public Worker(string name, string surname, string email, int salary)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Salary = salary;
        }
        
        public override string ToString()
        {
            return $"Name: {Name}Surname: {Surname}\tEmail: {Email}\tSalary: {Salary}";
        }
        
        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO workers ('name', 'surname', 'salary', 'email') VALUES (@name, @surname, @salary, @email)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@surname", Surname);
            backupCommand.Parameters.AddWithValue("@surname", Surname);
            
            command.Parameters.AddWithValue("@salary", Salary);
            backupCommand.Parameters.AddWithValue("@salary", Salary);
            
            command.Parameters.AddWithValue("@email", Email);
            backupCommand.Parameters.AddWithValue("@email", Email);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}