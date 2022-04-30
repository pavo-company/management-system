using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version26042022234010 : Migration
    {
        override 
            protected string Info => "Add Extractions";
        
        public override void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO extractions (worker_id, item_id, amount, user_id) " +
                "VALUES (1, 2, 1, 1)", 
                connection);
            ExecuteQuery(
                "INSERT INTO extractions (worker_id, item_id, amount, user_id) " +
                "VALUES (1, 4, 4, 2)", 
                connection);
            ExecuteQuery(
                "INSERT INTO extractions (worker_id, item_id, amount, user_id) " +
                "VALUES (2, 1, 1, 3)", 
                connection);
            ExecuteQuery(
                "INSERT INTO extractions (worker_id, item_id, amount, user_id) " +
                "VALUES (3, 3, 1, 4)", 
                connection);
            ExecuteQuery(
                "INSERT INTO extractions (worker_id, item_id, amount, user_id) " +
                "VALUES (4, 2, 2, 2)", 
                connection);
        }
       
        public override void Down(SQLiteConnection connection)
        {
            ExecuteQuery("drop table extractions;", connection);
        }
    }
}