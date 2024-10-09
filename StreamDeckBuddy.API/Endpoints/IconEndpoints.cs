using StreamDeckBuddy.API.DTOs;
using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services;
using StreamDeckBuddy.Services.Interfaces;

namespace StreamDeckBuddy.API.Endpoints;

public static class IconEndpoints
{
    public static RouteGroupBuilder MapIconEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", (int page, int pageSize, IIconService iconService) => 
        {
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest("Page and pageSize must be greater than zero.");
            }

            var icons = iconService
                .GetIcons()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(IconDto.FromDomain);

            return Results.Ok(icons);
        })
        .WithName("GetIcons")
        .WithOpenApi();

        group.MapGet("/search", (string searchTerm, IIconService iconService) =>
            {
                return Results.Ok(iconService
                    .GetIcons()
                    .Where(IconMatchesSearchTerm)
                    .Select(IconDto.FromDomain)
                );

                bool IconMatchesSearchTerm(Icon icon) =>
                    icon.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    icon.FullPath.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
            })
            .WithName("SearchIcons")
            .WithOpenApi();

        group.MapGet("/{id:guid}", (Guid id, IIconService iconService) =>
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
                        _ => "application/octet-stream"
                    };
                }
            })
            .WithName("GetIconById")
            .WithOpenApi();
        
        return group;
    }
}