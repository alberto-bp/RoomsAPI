using System;
using Restaurant.Common.Entities;

namespace RoomsAPI.Entities
{
    public class Room : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsIndoor { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}