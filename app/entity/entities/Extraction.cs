using management_system.app.entity;
using System;
using System.Data.SQLite;

namespace management_system
{
    public class Extraction : Entity
    {
        public int Id { get; init; }
        public int WorkerId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }

        public int GetId() => Id;
        public string DatabaseTableName() => "extractions";
        public string[] DatabaseColumnNames() => new string[] { "worker_id", "item_id", "amount", "user_id" };
        public string[] DatabaseColumnValues() => new string[] { $"{WorkerId}", $"{ItemId}", $"{Amount}", $"{UserId}" };


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


    }
}