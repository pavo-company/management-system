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
        public int GetId();
        public string DatabaseTableName();
        public string[] DatabaseColumnNames();
        public string[] DatabaseColumnValues();
    }
}
