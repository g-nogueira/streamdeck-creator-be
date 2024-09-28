namespace StreamDeckBuddy.API.DTOs;

public class GetIconsResponse
{
    public Guid Id { get; set; }
    public required string Label { get; set; }
    public required string Url { get; set; }
}