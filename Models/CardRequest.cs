namespace CardRequestApi.Models;

public class CardRequest
{
    public int Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public CardType CardType { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public CardRequestStatus Status { get; set; } = CardRequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}