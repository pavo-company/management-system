using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace management_system.app.entity
{
    public interface Entity
    {
        public string[] DatabaseColumnNames();
        public string[] DatabaseColumnValues();
        public void UpdateDatabase(Database db);
        public void AddToDatabase(Database db);
        
    }
}
