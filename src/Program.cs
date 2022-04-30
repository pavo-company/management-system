using System;
using migrations;

namespace management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            Migrations migration = new Migrations(db.Connection, db.BackupConnection);
            migration.MigrateAll();
        }
    }
}