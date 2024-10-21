using System.IO.Compression;
using StreamDeckBuddy.API.DTOs;
using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;

namespace StreamDeckBuddy.API.Endpoints;

public static class UserIconCollectionEndpoints
{
    public static RouteGroupBuilder MapUserIconCollectionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", (IUserIconCollectionService collectionService) => collectionService.GetList())
            .WithName("GetUserCollections")
            .WithOpenApi();

        group.MapPost("/", (UserIconCollection collection, IUserIconCollectionService collectionService) =>
            {
                if (collection.Id != new UserIconCollectionId(Guid.Empty)) {
                    return Results.BadRequest("Collection ID must be empty Guid");
                }
                
                var id = collectionService.Add(collection);
                return Results.Created($"/collections/{id}", id);
            })
            .WithName("AddUserCollection")
            .WithOpenApi();

        group.MapPost("/{id:guid}/icons", (Guid id, UpdateCollectionRequestDto.StylizedIconDto icon, IUserIconCollectionService collectionService) =>
            {
                var iconId = collectionService.AddIconToCollection(id, icon.ToDomain());
                return Results.Created($"/collections/{id}/icons/{icon.Id}", iconId);
            })
            .WithName("AddIconToUserCollection")
            .WithOpenApi();

        group.MapGet("/{id:guid}", (Guid id, IUserIconCollectionService collectionService) =>
            {
                var collection = collectionService.GetById(id);
                return collection is not null ? Results.Ok(collection) : Results.NotFound();
            })
            .WithName("GetUserCollectionById")
            .WithOpenApi();

        group.MapPut("/{id:guid}",
                (Guid id, UpdateCollectionRequestDto updatedCollection, IUserIconCollectionService collectionService) =>
                {
                    if (id == Guid.Empty) {
                        return Results.BadRequest("Collection ID must not be empty Guid");
                    }

                    var result = collectionService.UpdateCollection(id, updatedCollection.ToDomain());
                    return Results.Ok(result);
                })
            .WithName("UpdateUserCollection")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", (Guid id, IUserIconCollectionService collectionService) =>
            {
                collectionService.DeleteCollection(id);
                return Results.NoContent();
            })
            .WithName("DeleteUserCollection")
            .WithOpenApi();
        
        group.MapGet("{id:guid}/download", async (Guid id, IUserIconCollectionService collectionService) =>
            {
                var collection = collectionService.GetById(id);
                if (collection is null) return Results.NotFound();

                var memoryStream = new MemoryStream();
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var icon in collection.Icons)
                    {
                        var zipEntry = zipArchive.CreateEntry($"{icon.Label}.png");
                        await using var zipEntryStream = zipEntry.Open();
            
                        var base64String = icon.PngData.Replace("data:image/png;base64,", "");
                        // Decode the base64 string to a byte array
                        var imageBytes = Convert.FromBase64String(base64String);
            
                        // Write the byte array to the zip entry
                        await zipEntryStream.WriteAsync(imageBytes);
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                return Results.File(memoryStream, "application/zip", $"collection_{collection.Id}.zip");
            })
            .WithName("DownloadUserCollection")
            .WithOpenApi();
        
        return group;
    }
}