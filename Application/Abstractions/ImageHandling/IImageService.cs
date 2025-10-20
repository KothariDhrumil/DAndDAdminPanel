using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.ImageHandling
{
    public interface IImageService
    {
        Task<(string imageWebPath, string thumbnailWebPath)> SaveImageAsync(IFormFile imageFile, CancellationToken ct);
    }
}
