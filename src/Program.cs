using System;

namespace management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("John", "Smith","john@gmail.com");
            User user1 = new User("John1", "Smith", "john@gmail.com");
            User user2 = new User("John2", "Smith", "john@gmail.com");
            
            Console.WriteLine(user);
            Console.WriteLine(user1);
            Console.WriteLine(user2);

            Database db = new Database();
            user.AddToDatabase(db);
            user1.AddToDatabase(db);
            user2.AddToDatabase(db);
            Console.ReadKey();
        }
    }
}