using System.Data.SQLite;
using migration;

namespace migration.version
{
    public class Version26042022010810 : Migration
    {
        override 
            protected string Info=> "Add Users";

        override 
        public void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John', 'Smith', '900-000-000')", 
                connection);
            ExecuteQuery(
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John1', 'Smith1', '900-000-001')", 
                connection);
            ExecuteQuery(
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John2', 'Smith2', '900-000-002')", 
                connection);
            ExecuteQuery(
                "INSERT INTO users ('name', 'surname', 'tin') VALUES ('John3', 'Smith3', '900-000-003')", 
                connection);
        }
        
        override 
        public void Down(SQLiteConnection connection)
        {
            ExecuteQuery("DROP TABLE users", connection);
            string deleteUsers = 
                "DELETE FROM users WHERE name LIKE '%John%';";
            SQLiteCommand deleteUsersCommand = new SQLiteCommand(deleteUsers, connection);
            
            deleteUsersCommand.ExecuteNonQuery();
        }
    }
}