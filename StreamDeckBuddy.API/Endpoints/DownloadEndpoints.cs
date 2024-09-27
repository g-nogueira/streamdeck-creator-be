namespace StreamDeckBuddy.API.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StreamDeckBuddy.Models;
using System.IO;
using System.Threading.Tasks;

public static class DownloadEndpoints
{
    public static void MapDownloadEndpoints(this IEndpointRouteBuilder endpoints, List<Icon> icons, List<Collection> collections)
    {
        endpoints.MapGet("/download/icon/{id}", (int id) =>
        {
            var icon = icons.FirstOrDefault(i => i.Id == id);
            if (icon is null)
            {
                return Results.NotFound();
            }

            // Generate icon image (placeholder logic)
            byte[] imageBytes = GenerateIconImage(icon);
            return Results.File(imageBytes, "image/png", $"icon_{icon.Id}.png");
        })
        .WithName("DownloadIcon")
        .WithOpenApi();

        endpoints.MapGet("/download/collection/{id}", async (int id) =>
        {
            var collection = collections.FirstOrDefault(c => c.Id == id);
            if (collection is null)
            {
                return Results.NotFound();
            }

            using var memoryStream = new MemoryStream();
            using (var zipArchive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var icon in collection.Icons)
                {
                    // Generate icon image (placeholder logic)
                    byte[] imageBytes = GenerateIconImage(icon);
                    var zipEntry = zipArchive.CreateEntry($"icon_{icon.Id}.png");
                    using var zipEntryStream = zipEntry.Open();
                    await zipEntryStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Results.File(memoryStream, "application/zip", $"collection_{collection.Id}.zip");
        })
        .WithName("DownloadCollection")
        .WithOpenApi();
    }

    private static byte[] GenerateIconImage(Icon icon)
    {
        // Placeholder logic for generating an icon image
        return new byte[0];
    }
}