using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;

namespace StreamDeckBuddy.API.Endpoints;

public static class IconEndpoints
{
    public static void MapIconEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/icons", (IIconService iconService) =>
            {
                var icons = iconService.GetIcons().GetRange(0, 500);
                var iconFiles = icons.Select(icon => new
                {
                    icon.Id,
                    icon.Glyph,
                    icon.Label,
                    icon.CollectionId,
                    Content = File.ReadAllText(icon.FullPath) // Read SVG content as string
                });
                return Results.Ok(iconFiles);
            })
            .WithName("GetIcons")
            .WithOpenApi();

        endpoints.MapGet("/icons/search", (string searchTerm, IIconService iconService) =>
            {
                var icons = iconService.GetIcons();
                var filteredIcons = icons.Where(i => i.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                var iconFiles = filteredIcons.Select(icon => new
                {
                    icon.Id,
                    icon.Glyph,
                    icon.Label,
                    icon.CollectionId,
                    Content = File.ReadAllText(icon.FullPath) // Read SVG content as string
                });
                return Results.Ok(iconFiles);
            })
            .WithName("SearchIcons")
            .WithOpenApi();

        endpoints.MapPost("/icons", (Icon icon, IIconService iconService) =>
            {
                iconService.AddIcon(icon);
                return Results.Created($"/icons/{icon.Id}", icon);
            })
            .WithName("CreateIcon")
            .WithOpenApi();

        endpoints.MapGet("/icons/{id}", (int id, IIconService iconService) =>
            {
                var icon = iconService.GetIconById(id);
                return icon is not null ? Results.Ok(icon) : Results.NotFound();
            })
            .WithName("GetIconById")
            .WithOpenApi();

        endpoints.MapPut("/icons/{id}", (int id, Icon updatedIcon, IIconService iconService) =>
            {
                iconService.UpdateIcon(id, updatedIcon);
                return Results.Ok(updatedIcon);
            })
            .WithName("UpdateIcon")
            .WithOpenApi();

        endpoints.MapDelete("/icons/{id}", (int id, IIconService iconService) =>
            {
                iconService.DeleteIcon(id);
                return Results.NoContent();
            })
            .WithName("DeleteIcon")
            .WithOpenApi();

        endpoints.MapGet("/icons/{id}/file", (int id, IIconService iconService) =>
            {
                var icon = iconService.GetIconById(id);
                if (icon is null || !File.Exists(icon.FullPath)) return Results.NotFound();

                var fileContent = File.ReadAllText(icon.FullPath); // Read SVG content as string
                return Results.Text(fileContent, "image/svg+xml");
            })
            .WithName("GetIconFile")
            .WithOpenApi();
    }
}