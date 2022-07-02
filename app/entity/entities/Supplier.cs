using management_system.app.entity;
using System.Data.SQLite;

namespace management_system
{
    public class Supplier : Person, Entity
    {
        public Supplier(string name, string tin)
        {
            Id = -1;
            Name = name;
            Tin = tin;
        }
        public Supplier(int id, string name, string tin)
        {
            Id = id;
            Name = name;
            Tin = tin;
        }
        public int GetId() => Id;
        public string DatabaseTableName() => "suppliers";
        public string[] DatabaseColumnNames() => new string[] { "name", "tin" };
        public string[] DatabaseColumnValues() => new string[] { $"'{Name}'", $"'{Tin}'" };
    }
}