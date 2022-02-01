using API.Enums;

namespace API.Dtos
{
    public class PlayerResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public PlayerPositions Position { get; set; }
        public double Value { get; set; }
        public Guid TeamId { get; set; }
        public TeamResponseDto Team { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}