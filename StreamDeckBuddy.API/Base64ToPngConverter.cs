namespace StreamDeckBuddy.API;

using System;
using System.IO;

public class Base64ToPngConverter
{
    public static void ConvertBase64ToPng(string base64String, string outputFilePath)
    {
        // Remove the data:image/png;base64, prefix if it exists
        if (base64String.StartsWith("data:image/png;base64,"))
        {
            base64String = base64String.Substring("data:image/png;base64,".Length);
        }

        // Convert base64 string to byte array
        byte[] imageBytes = Convert.FromBase64String(base64String);

        // Write byte array to file
        File.WriteAllBytes(outputFilePath, imageBytes);
    }
}