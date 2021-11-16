using RoomsAPI.DTOs;
using RoomsAPI.Entities;

namespace RoomsAPI.Extensions
{
    public static class RoomExtensions
    {
        public static RoomDto AsDto(this Room room)
        {
            return new RoomDto(room.Id, room.Name, room.Capacity, room.IsIndoor);
        }
    }
}