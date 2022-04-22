using System;

namespace management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();

            db.PrintAllUsers();
            db.PrintAllWorkers();
        }
    }
}