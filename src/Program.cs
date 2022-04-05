using System;

namespace management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("John", "john@gmail.com");
            Console.WriteLine(user);
        }
    }
}