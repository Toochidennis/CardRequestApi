using CardRequestApi.DTOs;
using CardRequestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardRequestApi.Controllers;

[ApiController]
[Route("api/card-requests")]
public class CardRequestsController : ControllerBase
{
    private readonly CardRequestService _service;

    public CardRequestsController(CardRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCardRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetStatus),
            new { referenceNumber = result.ReferenceNumber },
            result
        );
    }

    [HttpGet("{referenceNumber}/status")]
    public async Task<IActionResult> GetStatus(
        string referenceNumber)
    {
        var result = await _service.GetStatusAsync(referenceNumber);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Card request not found"
            });
        }

        return Ok(result);
    }
}