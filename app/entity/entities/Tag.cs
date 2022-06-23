using management_system.app.entity;
using System;
using System.Data.SQLite;

namespace management_system
{
    public class Tag : Entity
    {
        public int Id { get; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string[] DatabaseColumnNames() => new string[] { "name", "item_id" };
        public string[] DatabaseColumnValues() => new string[] { $"'{Name}'", $"'{ItemId}'" };

        public Tag(string name, int itemId)
        {
            Id = -1;
            Name = name;
            ItemId = itemId;
        }

        public Tag(int id, string name, int itemId)
        {
            Id = id;
            Name = name;
            ItemId = itemId;
        }

        public void AddToDatabase(Database db) => db.em.AddTag(this);
        public void UpdateDatabase(Database db) => db.em.UpdateTag(this);
    }
}