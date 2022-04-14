using System;

namespace management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("John", "john@gmail.com");
            User user1 = new User("John", "john@gmail.com");
            User user2 = new User("John", "john@gmail.com");
            
            Console.WriteLine(user);
            Console.WriteLine(user1);
            Console.WriteLine(user2);
        }
    }
}