using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;

namespace StreamDeckBuddy.API.Endpoints;

public static class CollectionEndpoints
{
    public static void MapCollectionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/collections", (ICollectionService collectionService) => collectionService.GetCollections())
            .WithName("GetCollections")
            .WithOpenApi();

        endpoints.MapPost("/collections", (Collection collection, ICollectionService collectionService) =>
            {
                collectionService.AddCollection(collection);
                return Results.Created($"/collections/{collection.Id}", collection);
            })
            .WithName("CreateCollection")
            .WithOpenApi();

        endpoints.MapGet("/collections/{id}", (Guid id, ICollectionService collectionService) =>
            {
                var collection = collectionService.GetCollectionById(id);
                return collection is not null ? Results.Ok(collection) : Results.NotFound();
            })
            .WithName("GetCollectionById")
            .WithOpenApi();

        endpoints.MapPut("/collections/{id}",
                (Guid id, Collection updatedCollection, ICollectionService collectionService) =>
                {
                    collectionService.UpdateCollection(id, updatedCollection);
                    return Results.Ok(updatedCollection);
                })
            .WithName("UpdateCollection")
            .WithOpenApi();

        endpoints.MapDelete("/collections/{id}", (Guid id, ICollectionService collectionService) =>
            {
                collectionService.DeleteCollection(id);
                return Results.NoContent();
            })
            .WithName("DeleteCollection")
            .WithOpenApi();
    }
}