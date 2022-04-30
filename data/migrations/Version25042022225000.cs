using System;
using System.Data.SQLite;

namespace migration.version
{
    public class Version25042022225000 : Migration
    {
        override 
            protected string Info => "Add License table";

        override 
        public void Up(SQLiteConnection connection)
        {
            string createLicenseTable = 
                "create table license (id INTEGER PRIMARY KEY AUTOINCREMENT, expiry_date DATE);";
            SQLiteCommand addLicenseTable = new SQLiteCommand(createLicenseTable, connection);
            addLicenseTable.ExecuteNonQuery();
        }
        override 
        public void Down(SQLiteConnection connection)
        {
            string dropLicenseTable = 
                "drop table license;";
            SQLiteCommand deleteLicenseTable = new SQLiteCommand(dropLicenseTable, connection);
            deleteLicenseTable.ExecuteNonQuery();
        }
    }
}