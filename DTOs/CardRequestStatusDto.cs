namespace CardRequestApi.DTOs;
using CardRequestApi.Models;

public class CardRequestStatusDto
{
    public string ReferenceNumber { get; set; } = string.Empty;

    public CardRequestStatus Status { get; set; }
}