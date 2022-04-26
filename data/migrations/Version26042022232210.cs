using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version26042022232210 : Migration
    {
        public override String Info => "Add Orders";
        
        public override void Up(SQLiteConnection connection)
        {
            ExecuteQuery(
                "INSERT INTO orders (supplier_id, item_id, amount, date, is_cyclic) " +
                "VALUES (1, 2, 3, '30/04/2022', 1)", 
                connection);
            ExecuteQuery(
                "INSERT INTO orders (supplier_id, item_id, amount, date, is_cyclic) " +
                "VALUES (1, 4, 10, '10/05/2022', 0)", 
                connection);
            ExecuteQuery(
                "INSERT INTO orders (supplier_id, item_id, amount, date, is_cyclic) " +
                "VALUES (2, 1, 2, '30/04/2022', 0)", 
                connection);
            ExecuteQuery(
                "INSERT INTO orders (supplier_id, item_id, amount, date, is_cyclic) " +
                "VALUES (3, 3, 2, '15/06/2022', 1)", 
                connection);
            ExecuteQuery(
                "INSERT INTO orders (supplier_id, item_id, amount, date, is_cyclic) " +
                "VALUES (4, 2, 4, '23/05/2022', 1)", 
                connection);
        }
       
        public override void Down(SQLiteConnection connection)
        {
            ExecuteQuery("drop table orders;", connection);
        }
    }
}