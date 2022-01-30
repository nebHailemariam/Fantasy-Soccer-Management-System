using API.Enums;
using API.Helpers.Filter.Attributes;

namespace API.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [FilterAttribute]
        public string Country { get; set; }
        [FilterPlayerPositionAttribute]
        public PlayerPositions Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Value { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}