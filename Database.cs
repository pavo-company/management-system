using System.Data.SQLite;
using System.IO;

namespace management_system
{
    public class Database
    {
        public SQLiteConnection Connection;

        public Database()
        {
            if (!File.Exists("../../../database.sqlite"))
            {
                SQLiteConnection.CreateFile("../../../database.sqlite");
            }

            Connection = new SQLiteConnection("Data Source=../../../database.sqlite");
        }
    }
}