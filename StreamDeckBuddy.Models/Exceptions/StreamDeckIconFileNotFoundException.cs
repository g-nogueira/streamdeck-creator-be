namespace StreamDeckBuddy.Models.Exceptions;

public class StreamDeckIconFileNotFoundException(string filePath) : Exception($"Icon file not found at {filePath}");