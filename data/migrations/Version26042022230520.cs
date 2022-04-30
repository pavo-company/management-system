using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version26042022230520 : Migration
    {
        override 
            protected string Info => "Add Workers";
        
        public override void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO workers (name, surname, salary, tin) VALUES ('John', 'Smith', 30000, '900-000-0000')", connection);
            ExecuteQuery(
                "INSERT INTO workers (name, surname, salary, tin) VALUES ('John1', 'Smith1', 31000, '900-000-0001')", connection);
            ExecuteQuery(
                "INSERT INTO workers (name, surname, salary, tin) VALUES ('John2', 'Smith2', 32000, '900-000-0002')", connection);
            ExecuteQuery(
                "INSERT INTO workers (name, surname, salary, tin) VALUES ('John3', 'Smith3', 33000, '900-000-0003')", connection);
        }
       
       public override void Down(SQLiteConnection connection)
        {
            ExecuteQuery("drop table workers;", connection);
        }
    }
}