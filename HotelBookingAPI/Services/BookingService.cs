using HotelBookingAPI.Data;
using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Services
{
    public interface IBookingService
    {
        Task<Booking> BookRoom(int userId, BookingDTO dto);
    }

    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> BookRoom(int userId, BookingDTO dto)
        {
            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null) throw new Exception("Room not found");

            var overlap = await _context.Bookings
                .AnyAsync(b =>
                    b.RoomId == dto.RoomId &&
                    b.CheckIn < dto.CheckOut &&
                    b.CheckOut > dto.CheckIn);

            if (overlap) throw new Exception("Room already booked for these dates");

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}
