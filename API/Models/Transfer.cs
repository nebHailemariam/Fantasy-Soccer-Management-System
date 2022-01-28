using API.Enums;

namespace API.Entities
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public ApplicationUser Seller { get; set; }
        public Guid? BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
        public double AskingPrice { get; set; }
        public PlayerTransferStatus PlayerTransferStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}