using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version26042022173610 : Migration
    {
        public override String Info => "Add Items and tags";

         
        public override void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO items ('name', 'amount', 'min_amount') VALUES ('Computer', '3', '1')", connection);
            ExecuteQuery(
                "INSERT INTO items ('name', 'amount', 'min_amount') VALUES ('Laptop', '5', '2')", connection);
            ExecuteQuery(
                "INSERT INTO items ('name', 'amount', 'min_amount') VALUES ('Tablet', '10', '1')", connection);
            ExecuteQuery(
                "INSERT INTO items ('name', 'amount', 'min_amount') VALUES ('Samsung Galaxy s22', '6', '3')", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('electronic equipment', 1)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('electronic equipment', 2)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('electronic equipment', 3)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('electronic equipment', 4)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('PC', 1)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('Laptops and tablets', 2)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('Laptops and tablets', 3)", connection);
            ExecuteQuery(
                "INSERT INTO tags (name, item_id) VALUES ('Phones', 4)", connection);
        }
       
       public override void Down(SQLiteConnection connection)
        {
            ExecuteQuery("drop table items;", connection);
            ExecuteQuery("drop table tags;", connection);
        }
    }
}