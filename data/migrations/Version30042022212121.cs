using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version30042022212121 : Migration
    {
        override 
            protected string Info => "Add column price to items table";

        override 
        public void Up(SQLiteConnection connection)
        {
            string addColumnToTable = 
                "ALTER TABLE items ADD price INTEGER DEFAULT 0;";
            SQLiteCommand addLicenseTable = new SQLiteCommand(addColumnToTable, connection);
            addLicenseTable.ExecuteNonQuery();
        }
        override 
        public void Down(SQLiteConnection connection)
        {
            string dropColumnFromTable = 
                "ALTER TABLE items DROP COLUMN price;";
            SQLiteCommand deleteLicenseTable = new SQLiteCommand(dropColumnFromTable, connection);
            deleteLicenseTable.ExecuteNonQuery();
        }
    }
}