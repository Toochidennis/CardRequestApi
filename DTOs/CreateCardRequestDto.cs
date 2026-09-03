using System.ComponentModel.DataAnnotations;
using CardRequestApi.Models;

namespace CardRequestApi.DTOs;

public class CreateCardRequestDto
{
    [Required]
    [RegularExpression(
        @"^\d{10}$",
        ErrorMessage = "Account number must contain exactly 10 digits."
    )]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^(Debit|Credit|Prepaid)$",
        ErrorMessage = "Card type must be either 'Debit', 'Credit', or 'Prepaid'."
    )]
    public string CardType { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string DeliveryAddress { get; set; } = string.Empty;
}
