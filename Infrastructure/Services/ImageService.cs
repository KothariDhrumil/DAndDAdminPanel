using Application.Abstractions.ImageHandling;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public async Task<(string imageWebPath, string thumbnailWebPath)> SaveImageAsync(IFormFile imageFile, CancellationToken ct)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var imageDir = Path.Combine("wwwroot", "images", "products");
            Directory.CreateDirectory(imageDir);
            var imagePath = Path.Combine(imageDir, fileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, ct);
            }
            var imageWebPath = $"/images/products/{fileName}";
            var thumbFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_thumb{Path.GetExtension(fileName)}";
            var thumbDir = Path.Combine(imageDir, "thumbnails");
            Directory.CreateDirectory(thumbDir);
            var thumbnailPath = Path.Combine(thumbDir, thumbFileName);
            using (var image = await Image.LoadAsync(imagePath, ct))
            {
                image.Mutate(x => x.Resize(150, 150));
                await image.SaveAsync(thumbnailPath, ct);
            }
            var thumbnailWebPath = $"/images/products/thumbnails/{thumbFileName}";
            return (imageWebPath, thumbnailWebPath);
        }
    }
}
