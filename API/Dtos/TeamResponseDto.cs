namespace API.Dtos
{
    public class TeamResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double TeamValue { get; set; }
        public double Money { get; set; }
        public Guid OwnerId { get; set; }
        public UserDto Owner { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}