using System;
using System.Data.SQLite;
using management_system;

namespace migration
{
    public class Migration
    {
        public virtual String Info => "Default migration";
        public virtual void Up(SQLiteConnection connection){}
        public virtual void Down(SQLiteConnection connection){}
        
        public void AddToDatabase(SQLiteConnection Connection)
        {
            string query = 
                $"INSERT INTO migrations ('version', 'deployed_at', 'info') VALUES ('{this.GetType().Name}', '{DateTime.Now.ToString("yyyy-MM-dd")}', '{this.Info}');";

            SQLiteCommand command = new SQLiteCommand(query,Connection);
            
            command.ExecuteNonQuery();
            
        }
    }
}