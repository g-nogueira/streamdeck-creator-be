namespace StreamDeckBuddy.API.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;

public static class IconEndpoints
{
    public static void MapIconEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/icons", (IFileSystemIconService iconService) => iconService.GetIcons())
            .WithName("GetIcons")
            .WithOpenApi();

        endpoints.MapGet("/icons/search", (string searchTerm, IFileSystemIconService iconService) =>
        {
            var icons = iconService.GetIcons();
            var filteredIcons = icons.Where(i => i.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            return Results.Ok(filteredIcons);
        });

        endpoints.MapPost("/icons", (Icon icon, IFileSystemIconService iconService) =>
        {
            iconService.AddIcon(icon);
            return Results.Created($"/icons/{icon.Id}", icon);
        })
        .WithName("CreateIcon")
        .WithOpenApi();

        endpoints.MapGet("/icons/{id}", (int id, IFileSystemIconService iconService) =>
        {
            var icon = iconService.GetIconById(id);
            return icon is not null ? Results.Ok(icon) : Results.NotFound();
        })
        .WithName("GetIconById")
        .WithOpenApi();

        endpoints.MapPut("/icons/{id}", (int id, Icon updatedIcon, IFileSystemIconService iconService) =>
        {
            iconService.UpdateIcon(id, updatedIcon);
            return Results.Ok(updatedIcon);
        })
        .WithName("UpdateIcon")
        .WithOpenApi();

        endpoints.MapDelete("/icons/{id}", (int id, IFileSystemIconService iconService) =>
        {
            iconService.DeleteIcon(id);
            return Results.NoContent();
        })
        .WithName("DeleteIcon")
        .WithOpenApi();
    }
}