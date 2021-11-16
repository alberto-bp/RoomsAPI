using System;

namespace RoomsAPI.DTOs
{
    public record RoomDto(Guid Id, string Name, int Capacity, bool IsIndoor);
    public record CreateRoomDto(string Name, int Capacity, bool IsIndoor);
    public record UpdateRoomDto(string Name, int Capacity, bool IsIndoor);
}