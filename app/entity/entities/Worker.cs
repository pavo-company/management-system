using management_system.app.entity;
using System;
using System.Data.SQLite;

namespace management_system
{
    public class Worker : Person, Entity
    {
        public int Salary { get; init; }
        public int GetId() => Id;
        public string DatabaseTableName() => "workers";
        public string[] DatabaseColumnNames() => new string[] { "name", "surname", "salary", "tin" };
        public string[] DatabaseColumnValues() => new string[] { $"'{Name}'", $"'{Surname}'", $"{Salary}", $"'{Tin}'" };

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


    }
}