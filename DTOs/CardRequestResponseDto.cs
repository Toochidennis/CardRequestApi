namespace CardRequestApi.DTOs;
using CardRequestApi.Models;

public class CardRequestResponseDto
{
    public string ReferenceNumber { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public CardType CardType { get; set; }

    public CardRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}