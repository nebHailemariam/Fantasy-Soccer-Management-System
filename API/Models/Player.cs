using API.Enums;

namespace API.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public PlayerPositions Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Value { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}