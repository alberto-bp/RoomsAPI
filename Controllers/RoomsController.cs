using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Common.Contracts;
using Restaurant.Common.Repositories;
using RoomsAPI.DTOs;
using RoomsAPI.Entities;
using RoomsAPI.Extensions;

namespace RoomsAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRepository<Room> repository;
        private readonly IPublishEndpoint publishEndpoint;
        public RoomsController(IRepository<Room> repository, IPublishEndpoint publishEndpoint)
        {
            this.repository = repository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            return (await repository.GetAllAsync()).Select(r => r.AsDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetAsync(Guid id)
        {
            var room = await repository.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> PostAsync(CreateRoomDto newRoom)
        {
            var room = new Room {
                Name = newRoom.Name,
                Capacity = newRoom.Capacity,
                IsIndoor = newRoom.IsIndoor,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateAsync(room);

            await publishEndpoint.Publish(new RoomCreated(room.Id, room.Name, room.Capacity));

            return CreatedAtAction(nameof(GetAsync), new { id = room.Id }, room.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateRoomDto updatedRoom)
        {
            var existingRoom = await repository.GetAsync(id);

            if (existingRoom is null)
            {
                return NotFound();
            }

            existingRoom.Capacity = updatedRoom.Capacity;
            existingRoom.Name = updatedRoom.Name;
            existingRoom.IsIndoor = updatedRoom.IsIndoor;

            await repository.UpdateAsync(existingRoom);

            await publishEndpoint.Publish(new RoomUpdated(existingRoom.Id, existingRoom.Name, existingRoom.Capacity));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var room = await repository.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(id);

            await publishEndpoint.Publish(new RoomDeleted(room.Id));

            return NoContent();
        }
    }
}