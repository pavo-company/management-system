using System;
using System.Data.SQLite;

namespace management_system
{
    public class Extraction
    {
        public int Id { get; init; }
        public int WorkerId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }

        public Extraction(int workerId, int itemId, int userId, int amount)
        {
            Id = -1;
            WorkerId = workerId;
            ItemId = itemId;
            UserId = userId;
            Amount = amount;
        }

        public Extraction(int id, int workerId, int itemId, int userId, int amount)
        {
            Id = id;
            WorkerId = workerId;
            ItemId = itemId;
            UserId = userId;
            Amount = amount;
        }

        public void AddToDatabase(Database db) => db.em.AddExtraction(this);

    }
}