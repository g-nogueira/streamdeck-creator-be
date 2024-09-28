using StreamDeckBuddy.API.DTOs;
using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;

namespace StreamDeckBuddy.API.Endpoints;

using System.Linq;

public static class IconEndpoints
{
    public static void MapIconEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/icons", (IIconService iconService) =>
            {
                return Results.Ok(iconService
                    .GetIcons()
                    .Take(500)
                    .Select(icon =>
                        new GetIconsResponse
                        {
                            Id = icon.Id,
                            Label = icon.Label,
                            Url = File.ReadAllText(icon.FullPath),
                        }
                    )
                );
            })
            .WithName("GetIcons")
            .WithOpenApi();

        endpoints.MapGet("/icons/search", (string searchTerm, IIconService iconService) =>
            {
                return Results.Ok(iconService
                    .GetIcons()
                    .Where(IconMatchesSearchTerm)
                    .Select(icon => new
                    {
                        icon.Id,
                        icon.Label,
                        Content = File.ReadAllText(icon.FullPath) // Read SVG content as string
                    }));

                bool IconMatchesSearchTerm(Icon icon) =>
                    icon.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    icon.FullPath.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
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

        endpoints.MapGet("/icons/{id:guid}", (Guid id, IIconService iconService) =>
            {
                var icon = iconService.GetIconById(id);

                if (icon is null) return Results.NotFound();

                var fileStream = new FileStream(icon.FullPath, FileMode.Open, FileAccess.Read);

                return Results.File(fileStream, GetContentType(icon.FullPath), enableRangeProcessing: true);

                string GetContentType(string filePath)
                {
                    var extension = Path.GetExtension(filePath).ToLowerInvariant();
                    return extension switch
                    {
                        ".svg" => "image/svg+xml",
                        ".png" => "image/png",
                        ".ico" => "image/x-icon",
                        ".jpeg" => "image/jpeg",
                        ".jpg" => "image/jpeg",
                        ".gif" => "image/gif",
                        _ => "application/octet-stream",
                    };
                }
            })
            .WithName("GetIconById")
            .WithOpenApi();

        endpoints.MapPut("/icons/{id:guid}", (Guid id, Icon updatedIcon, IIconService iconService) =>
            {
                iconService.UpdateIcon(id, updatedIcon);
                return Results.Ok(updatedIcon);
            })
            .WithName("UpdateIcon")
            .WithOpenApi();

        endpoints.MapDelete("/icons/{id:guid}", (Guid id, IIconService iconService) =>
            {
                iconService.DeleteIcon(id);
                return Results.NoContent();
            })
            .WithName("DeleteIcon")
            .WithOpenApi();
    }
}