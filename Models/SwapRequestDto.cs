namespace FashAI.Models
{
    public class SwapRequestDto
    {
        public string InitiatorUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public int InitiatorClothingId { get; set; }
        public int ReceiverClothingId { get; set; }
    }
}
