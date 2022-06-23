using System;
using System.Data.SQLite;

namespace management_system
{
    public class Person
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Tin { get; set; }
        
    }
}