using HotelBookingAPI.DTOs;

namespace HotelBookingAPI.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO dto);
        Task<string> Login(LoginDTO dto);
    }
}
