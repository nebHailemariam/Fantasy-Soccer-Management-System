using API.Enums;

namespace API.Dtos
{
    public class TransferResponseDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public UserDto Seller { get; set; }
        public Guid? BuyerId { get; set; }
        public UserDto Buyer { get; set; }
        public Guid PlayerId { get; set; }
        public PlayerResponseDto Player { get; set; }
        public double AskingPrice { get; set; }
        public PlayerTransferStatus PlayerTransferStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}