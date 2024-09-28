using StreamDeckBuddy.API.DTOs;
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
                var id = collectionService.AddCollection(collection);
                return Results.Created($"/collections/{id}", collection);
            })
            .WithName("AddCollection")
            .WithOpenApi();

        endpoints.MapPut("/collections/{collectionId:guid}/icons", (Guid collectionId,
                AddIconToCollectionResponse stylizedIcon, ICollectionService collectionService,
                IIconService iconService) =>
            {
                var icon = iconService.GetIconById(stylizedIcon.IconId);

                if (icon is null)
                    return Results.NotFound($"Icon with ID {stylizedIcon.IconId} not found");

                var domain = stylizedIcon.ToDomain(icon);

                collectionService.AddIconToCollection(collectionId, domain);
                return Results.Created($"/collections/{collectionId}/icons/{stylizedIcon.Id}", stylizedIcon);
            })
            .WithName("AddIconToCollection")
            .WithOpenApi();

        endpoints.MapGet("/collections/{id:guid}", (Guid id, ICollectionService collectionService) =>
            {
                var collection = collectionService.GetCollectionById(id);
                return collection is not null ? Results.Ok(collection) : Results.NotFound();
            })
            .WithName("GetCollectionById")
            .WithOpenApi();

        endpoints.MapPut("/collections/{id:guid}",
                (Guid id, Collection updatedCollection, ICollectionService collectionService) =>
                {
                    collectionService.UpdateCollection(id, updatedCollection);
                    return Results.Ok(updatedCollection);
                })
            .WithName("UpdateCollection")
            .WithOpenApi();

        endpoints.MapDelete("/collections/{id:guid}", (Guid id, ICollectionService collectionService) =>
            {
                collectionService.DeleteCollection(id);
                return Results.NoContent();
            })
            .WithName("DeleteCollection")
            .WithOpenApi();
    }
}