using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version29042022011210 : Migration
    {
        override 
        public String Info => "Add column license_key to license table";

        override 
        public void Up(SQLiteConnection connection)
        {
            string addColumnToTable = 
                "ALTER TABLE license ADD license_key STRING;";
            SQLiteCommand addLicenseTable = new SQLiteCommand(addColumnToTable, connection);
            addLicenseTable.ExecuteNonQuery();
        }
        override 
        public void Down(SQLiteConnection connection)
        {
            string dropColumnFromTable = 
                "ALTER TABLE license DROP COLUMN license_key;";
            SQLiteCommand deleteLicenseTable = new SQLiteCommand(dropColumnFromTable, connection);
            deleteLicenseTable.ExecuteNonQuery();
        }
    }
}