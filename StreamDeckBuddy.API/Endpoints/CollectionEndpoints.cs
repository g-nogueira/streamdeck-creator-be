namespace StreamDeckBuddy.API.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StreamDeckBuddy.Models;

public static class CollectionEndpoints
{
    public static void MapCollectionEndpoints(this IEndpointRouteBuilder endpoints, List<Collection> collections)
    {
        endpoints.MapGet("/collections", () => { return collections; })
            .WithName("GetCollections")
            .WithOpenApi();

        endpoints.MapPost("/collections", (Collection collection) =>
        {
            collection.Id = collections.Count > 0 ? collections.Max(c => c.Id) + 1 : 1;
            collections.Add(collection);
            return Results.Created($"/collections/{collection.Id}", collection);
        })
        .WithName("CreateCollection")
        .WithOpenApi();

        endpoints.MapGet("/collections/{id}", (int id) =>
        {
            var collection = collections.FirstOrDefault(c => c.Id == id);
            return collection is not null ? Results.Ok(collection) : Results.NotFound();
        })
        .WithName("GetCollectionById")
        .WithOpenApi();

        endpoints.MapPut("/collections/{id}", (int id, Collection updatedCollection) =>
        {
            var collection = collections.FirstOrDefault(c => c.Id == id);
            if (collection is null)
            {
                return Results.NotFound();
            }

            collection.Name = updatedCollection.Name;
            collection.Icons = updatedCollection.Icons;
            return Results.Ok(collection);
        })
        .WithName("UpdateCollection")
        .WithOpenApi();

        endpoints.MapDelete("/collections/{id}", (int id) =>
        {
            var collection = collections.FirstOrDefault(c => c.Id == id);
            if (collection is null)
            {
                return Results.NotFound();
            }

            collections.Remove(collection);
            return Results.NoContent();
        })
        .WithName("DeleteCollection")
        .WithOpenApi();
    }
}