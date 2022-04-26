using System.Data.SQLite;
using migration;

namespace migration.version
{
    public class Version26042022010810 : Migration
    {
        override 
        public string Info => "Add Users";

        override 
        public void Up(SQLiteConnection connection)
        {
            string addUsers1 = 
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John', 'Smith', '900-000-000')";
            string addUsers2 = 
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John1', 'Smith1', '900-000-001')";
            string addUsers3 = 
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John2', 'Smith2', '900-000-002')";
            string addUsers4 = 
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John3', 'Smith3', '900-000-003')";

            SQLiteCommand addUser1Command = new SQLiteCommand(addUsers1, connection);
            SQLiteCommand addUser2Command = new SQLiteCommand(addUsers2, connection);
            SQLiteCommand addUser3Command = new SQLiteCommand(addUsers3, connection);
            SQLiteCommand addUser4Command = new SQLiteCommand(addUsers4, connection);
            
            addUser1Command.ExecuteNonQuery();
            addUser2Command.ExecuteNonQuery();
            addUser3Command.ExecuteNonQuery();
            addUser4Command.ExecuteNonQuery();
        }
        
        override 
        public void Down(SQLiteConnection connection)
        {
            string deleteUsers = 
                "DELETE FROM users WHERE name LIKE '%';";
            SQLiteCommand deleteUsersCommand = new SQLiteCommand(deleteUsers, connection);
            
            deleteUsersCommand.ExecuteNonQuery();
        }
    }
}