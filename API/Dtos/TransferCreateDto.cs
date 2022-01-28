namespace API.Dtos
{
    public class TransferCreateDto
    {
        public Guid PlayerId { get; set; }
        public double AskingPrice { get; set; }
    }
}