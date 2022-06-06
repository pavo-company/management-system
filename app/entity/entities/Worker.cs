using System;
using System.Data.SQLite;

namespace management_system
{
    public class Worker : Person
    {
        public int Salary { get; init; }
         
        public Worker(string name, string surname, string tin, int salary)
        {
            Id = -1;
            Name = name;
            Surname = surname;
            Tin = tin;
            Salary = salary;
        }
        public Worker(int id, string name, string surname, string tin, int salary)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Tin = tin;
            Salary = salary;
        }
        
        public override string ToString() => $"Name: {Name}Surname: {Surname}\tTin: {Tin}\tSalary: {Salary}";

        public void AddToDatabase(Database db) => db.em.AddWorker(this);

    }
}