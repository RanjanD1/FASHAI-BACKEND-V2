namespace FashAI.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }
        public string InitiatorUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public int InitiatorClothingId { get; set; }
        public int ReceiverClothingId { get; set; }
        public string Status { get; set; }// e.g., "Pending", "Agreed", "Rejected"
    }
}
