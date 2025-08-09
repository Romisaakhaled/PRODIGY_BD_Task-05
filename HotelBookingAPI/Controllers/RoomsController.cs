using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] RoomSearchDTO dto)
        {
            var rooms = await _context.Rooms
                .Where(r => !_context.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    b.CheckIn < dto.CheckOut &&
                    b.CheckOut > dto.CheckIn))
                .Where(r => string.IsNullOrEmpty(dto.Location) || r.Location.Contains(dto.Location))
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoomDTO dto)
        {
            var room = new Room
            {
                Name = dto.Name,
                Location = dto.Location,
                Capacity = dto.Capacity
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return Ok(room);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, RoomDTO dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            room.Name = dto.Name;
            room.Location = dto.Location;
            room.Capacity = dto.Capacity;

            await _context.SaveChangesAsync();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
