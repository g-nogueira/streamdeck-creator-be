namespace StreamDeckBuddy.API.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StreamDeckBuddy.Models;

public static class IconEndpoints
{
    public static void MapIconEndpoints(this IEndpointRouteBuilder endpoints, List<Icon> icons)
    {
        endpoints.MapGet("/icons", () => { return icons; })
            .WithName("GetIcons")
            .WithOpenApi();

        endpoints.MapGet("/icons/search", (string searchTerm) =>
        {
            var filteredIcons = icons.Where(i => i.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            return Results.Ok(filteredIcons);
        });

        endpoints.MapPost("/icons", (Icon icon) =>
        {
            icon.Id = icons.Count > 0 ? icons.Max(i => i.Id) + 1 : 1;
            icons.Add(icon);
            return Results.Created($"/icons/{icon.Id}", icon);
        })
        .WithName("CreateIcon")
        .WithOpenApi();

        endpoints.MapGet("/icons/{id}", (int id) =>
        {
            var icon = icons.FirstOrDefault(i => i.Id == id);
            return icon is not null ? Results.Ok(icon) : Results.NotFound();
        })
        .WithName("GetIconById")
        .WithOpenApi();

        endpoints.MapPut("/icons/{id}", (int id, Icon updatedIcon) =>
        {
            var icon = icons.FirstOrDefault(i => i.Id == id);
            if (icon is null)
            {
                return Results.NotFound();
            }

            icon.Glyph = updatedIcon.Glyph;
            icon.Label = updatedIcon.Label;
            icon.Color = updatedIcon.Color;
            icon.CollectionId = updatedIcon.CollectionId;
            return Results.Ok(icon);
        })
        .WithName("UpdateIcon")
        .WithOpenApi();

        endpoints.MapDelete("/icons/{id}", (int id) =>
        {
            var icon = icons.FirstOrDefault(i => i.Id == id);
            if (icon is null)
            {
                return Results.NotFound();
            }

            icons.Remove(icon);
            return Results.NoContent();
        })
        .WithName("DeleteIcon")
        .WithOpenApi();
    }
}