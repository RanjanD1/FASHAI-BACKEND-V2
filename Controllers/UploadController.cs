using FashAI.Data;
using FashAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FashAI.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly FashAIContext _context;

        public UploadController(IWebHostEnvironment env, FashAIContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpPost("user-image")]
        public async Task<IActionResult> UploadUserImage([FromForm] IFormFile file, [FromForm] string userId)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var userImage = new UserImage { UserId = userId, ImageUrl = $"/uploads/{fileName}" };
            _context.UserImages.Add(userImage);
            await _context.SaveChangesAsync();

            return Ok(new { userImage.ImageUrl });
        }

        [HttpPost("clothing-image")]
        public async Task<IActionResult> UploadClothingImage([FromForm] IFormFile file, [FromForm] string clothingName)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var clothingItem = new ClothingItem { Name = clothingName, ImageUrl = $"/uploads/{fileName}" };
            _context.ClothingItems.Add(clothingItem);
            await _context.SaveChangesAsync();

            return Ok(new { clothingItem.ImageUrl });
        }
    }
}
