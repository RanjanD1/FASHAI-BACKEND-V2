using FashAI.Data;
using FashAI.Models;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;

namespace FashAI.Controllers
{
    [Route("api/tryon")]
    [ApiController]
    public class TryOnController : ControllerBase
    {
        private readonly FashAIContext _context;

        public TryOnController(FashAIContext context)
        {
            _context = context;
        }

        [HttpPost("preview")]
        public async Task<IActionResult> TryOnPreview([FromBody] TryOnDto tryOnDto)
        {
            var userImage = await _context.UserImages.FindAsync(tryOnDto.UserImageId);
            var clothingItem = await _context.ClothingItems.FindAsync(tryOnDto.ClothingItemId);

            if (userImage == null || clothingItem == null)
                return BadRequest("Invalid user image or clothing item");

            var userImageMat = Cv2.ImRead(userImage.ImageUrl);
            var clothingImageMat = Cv2.ImRead(clothingItem.ImageUrl, ImreadModes.Unchanged);

            //Resize clothing and overlay
            Mat resultImage = OverlayClothing(userImageMat, clothingImageMat);

            var outputFileName = $"tryon_{Guid.NewGuid()}.png";
            var outputPath = Path.Combine("wwwroot/tryon-previews", outputFileName);
            Cv2.ImWrite(outputPath, resultImage);

            return Ok(new { ImageUrl = $"/tryon-previews/{outputFileName}"});
        }

        private Mat OverlayClothing(Mat userImage, Mat clothingImage)
        {
            var scaleFactor = 0.5;
            Cv2.Resize(clothingImage, clothingImage, new Size(userImage.Width * scaleFactor, userImage.Height * scaleFactor));
            clothingImage.CopyTo(userImage[new Rect(100, 150, clothingImage.Width, clothingImage.Height)]);
            return userImage;
        }
    }
}
