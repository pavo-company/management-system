using System;
using System.Data.SQLite;

namespace management_system
{
    public class Worker : Person
    {
        private int Salary { get; init; }
         
        public Worker(string name, string surname, string tin, int salary)
        {
            Name = name;
            Surname = surname;
            Tin = tin;
            Salary = salary;
        }
        
        public override string ToString()
        {
            return $"Name: {Name}Surname: {Surname}\tEmail: {Tin}\tSalary: {Salary}";
        }
        
        public void AddToDatabase(Database db)
        {
            string query = 
                "INSERT INTO workers ('name', 'surname', 'salary', 'tin') VALUES (@name, @surname, @salary, @tin)";

            SQLiteCommand command = new SQLiteCommand(query, db.Connection);
            SQLiteCommand backupCommand = new SQLiteCommand(query, db.BackupConnection);
            
            db.Open();
            
            command.Parameters.AddWithValue("@name", Name);
            backupCommand.Parameters.AddWithValue("@name", Name);
            
            command.Parameters.AddWithValue("@surname", Surname);
            backupCommand.Parameters.AddWithValue("@surname", Surname);
            
            command.Parameters.AddWithValue("@salary", Salary);
            backupCommand.Parameters.AddWithValue("@salary", Salary);
            
            command.Parameters.AddWithValue("@tin", Tin);
            backupCommand.Parameters.AddWithValue("@tin", Tin);
            
            command.ExecuteNonQuery();
            backupCommand.ExecuteNonQuery();
            
            db.Close();
        }
    }
}