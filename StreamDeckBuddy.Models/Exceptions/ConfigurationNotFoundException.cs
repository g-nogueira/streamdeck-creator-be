namespace StreamDeckBuddy.Models.Exceptions;

public class ConfigurationNotFoundException(string configurationName)
    : Exception($"Configuration value for {configurationName} not found.");