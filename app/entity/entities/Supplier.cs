using System.Data.SQLite;

namespace management_system
{
    public class Supplier : Person
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

        public void AddToDatabase(Database db) => db.em.AddSupplier(this);
    }
}