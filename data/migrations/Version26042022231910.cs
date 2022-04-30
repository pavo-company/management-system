using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version26042022231910 : Migration
    {
        override 
            protected string Info => "Add Suppliers";
        
        public override void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO suppliers (name, tin) VALUES ('Media Expert', '900-000-0000')", connection);
            ExecuteQuery(
                "INSERT INTO suppliers (name, tin) VALUES ('Komputronik', '900-000-0000')", connection);
            ExecuteQuery(
                "INSERT INTO suppliers (name, tin) VALUES ('x-com', '900-000-0000')", connection);
            ExecuteQuery(
                "INSERT INTO suppliers (name, tin) VALUES ('RTV EURO AGD', '900-000-0000')", connection);
        }
       
        public override void Down(SQLiteConnection connection)
        {
            ExecuteQuery("drop table suppliers;", connection);
        }
    }
}