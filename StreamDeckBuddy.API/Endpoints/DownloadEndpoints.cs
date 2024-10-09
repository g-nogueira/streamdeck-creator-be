using System.IO.Compression;
using StreamDeckBuddy.Services;
using StreamDeckBuddy.Services.Interfaces;

namespace StreamDeckBuddy.API.Endpoints;

public static class DownloadEndpoints
{
    public static void MapDownloadEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/download/icon/{id}", (Guid id, IIconService iconService) =>
            {
                var icon = iconService.GetIconById(id);
                if (icon is null || !File.Exists(icon.FullPath)) return Results.NotFound();

                var fileBytes = File.ReadAllBytes(icon.FullPath);
                return Results.File(fileBytes, "image/svg+xml", Path.GetFileName(icon.FullPath));
            })
            .WithName("DownloadIcon")
            .WithOpenApi();

        endpoints.MapGet("/download/collection/{id}", async (Guid id, IUserIconCollectionService collectionService) =>
            {
                var collection = collectionService.GetById(id);
                if (collection is null) return Results.NotFound();

                using var memoryStream = new MemoryStream();
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // foreach (var icon in collection.Icons)
                    //     if (icon.PngData is not null)
                    //     {
                    //         var zipEntry = zipArchive.CreateEntry($"{icon.Label}.png");
                    //         using var zipEntryStream = zipEntry.Open();
                    //         await zipEntryStream.WriteAsync(icon.PngData, 0, icon.PngData.Length);
                    //     }
                    //     else
                    //     
                        // if (File.Exists(icon.FullPath))
                        // {
                        //     var fileBytes = File.ReadAllBytes(icon.FullPath);
                        //     var zipEntry = zipArchive.CreateEntry(Path.GetFileName(icon.FullPath));
                        //     using var zipEntryStream = zipEntry.Open();
                        //     await zipEntryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                        // }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                return Results.File(memoryStream, "application/zip", $"collection_{collection.Id}.zip");
            })
            .WithName("DownloadCollection")
            .WithOpenApi();
    }
}