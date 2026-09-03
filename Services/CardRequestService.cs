using CardRequestApi.Data;
using CardRequestApi.DTOs;
using CardRequestApi.Exceptions;
using CardRequestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CardRequestApi.Services;

public class CardRequestService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CardRequestService> _logger;

    public CardRequestService(AppDbContext context, ILogger<CardRequestService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CardRequestResponseDto> CreateAsync(CreateCardRequestDto dto)
    {
        var cardType = Enum.Parse<CardType>(dto.CardType, ignoreCase: true);

        var duplicateExists = await _context
            .CardRequests.AsNoTracking()
            .AnyAsync(x =>
                x.AccountNumber == dto.AccountNumber
                && x.CardType == cardType
                && (x.Status == CardRequestStatus.Pending || x.Status == CardRequestStatus.Approved)
            );

        if (duplicateExists)
        {
            throw new DuplicateCardRequestException(
                "An active card request already exists for this account and card type."
            );
        }

        var cardRequest = new CardRequest
        {
            ReferenceNumber = GenerateReferenceNumber(),
            AccountNumber = dto.AccountNumber,
            CustomerName = dto.CustomerName,
            CardType = Enum.Parse<CardType>(dto.CardType, ignoreCase: true),
            DeliveryAddress = dto.DeliveryAddress,
            Status = CardRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _context.CardRequests.Add(cardRequest);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Card request created successfully with reference {ReferenceNumber}",
            cardRequest.ReferenceNumber
        );

        return new CardRequestResponseDto
        {
            ReferenceNumber = cardRequest.ReferenceNumber,
            AccountNumber = cardRequest.AccountNumber,
            CustomerName = cardRequest.CustomerName,
            CardType = cardRequest.CardType,
            Status = cardRequest.Status,
            CreatedAt = cardRequest.CreatedAt,
        };
    }

    public async Task<CardRequestStatusDto?> GetStatusAsync(string referenceNumber)
    {
        var cardRequest = await _context
            .CardRequests.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ReferenceNumber == referenceNumber);

        if (cardRequest == null)
        {
            _logger.LogWarning(
                "Card request with reference {ReferenceNumber} was not found",
                referenceNumber
            );

            return null;
        }

        _logger.LogInformation(
            "Status retrieved for card request {ReferenceNumber}",
            referenceNumber
        );

        return new CardRequestStatusDto
        {
            ReferenceNumber = cardRequest.ReferenceNumber,
            Status = cardRequest.Status,
        };
    }

    private static string GenerateReferenceNumber()
    {
        var uniquePart = Guid.NewGuid().ToString("N")[..12].ToUpper();

        return $"CR-{DateTime.UtcNow:yyyyMMdd}-{uniquePart}";
    }
}
