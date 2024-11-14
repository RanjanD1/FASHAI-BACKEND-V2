using FashAI.Data;
using FashAI.Hubs;
using FashAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FashAI.Controllers
{
    [Route("api/swap")]
    [ApiController]
    [Authorize]//Only authenticated users can access these endpoints
    public class SwapController : ControllerBase
    {
        private readonly FashAIContext _context;
        private readonly IHubContext<SwapHub> _hubContext;

        public SwapController(FashAIContext context, IHubContext<SwapHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestSwap([FromBody] SwapRequestDto swapRequestDto)
        {
            var swapRequest = new SwapRequest
            {
                InitiatorUserId = swapRequestDto.InitiatorUserId,
                ReceiverUserId = swapRequestDto.ReceiverUserId,
                InitiatorClothingId = swapRequestDto.InitiatorClothingId,
                ReceiverClothingId = swapRequestDto.ReceiverClothingId,
                Status = "Pending"
            };

            _context.SwapRequests.Add(swapRequest);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(swapRequest.ReceiverUserId).SendAsync("ReceiveSwapRequest", "New swap Request!");

            return Ok(new { Message = "Swap request created", SwapRequestId = swapRequest.Id });
        }

        [HttpPost("respond")]
        public async Task<IActionResult> RespondToSwap([FromBody] SwapResponseDto responseDto)
        {
            var swapRequest = await _context.SwapRequests.FindAsync(responseDto.SwapRequestId);
            if (swapRequest == null)
                return NotFound("Swap request not found");

            swapRequest.Status = responseDto.IsAccepted ? "Agreed" : "Rejected";
            await _context.SaveChangesAsync();

            string message = responseDto.IsAccepted ? "Swap Request Accepted!" : "Swap Request Rejected!";
            await _hubContext.Clients.User(swapRequest.InitiatorUserId).SendAsync("Receive Swap Response", message);

            return Ok(new { Message = $"Swap {swapRequest.Status}" });
        }
    }
}
