namespace HotelBookingAPI.DTOs
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
    }

    public class RoomSearchDTO
    {
        public string Location { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
