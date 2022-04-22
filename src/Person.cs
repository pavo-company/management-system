using System;
using System.Data.SQLite;

namespace management_system
{
    public class Person
    {
        public int Id { get; init; }
        protected string Name { get; init; }
        protected string Surname { get; init; }
        protected string Email { get; init; }
        
    }
}