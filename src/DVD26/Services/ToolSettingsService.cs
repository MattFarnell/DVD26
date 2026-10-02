using System.IO;
using System.Text.Json;

namespace DVD26.Services;

public sealed record ToolSettings(string FfmpegPath = "", string FfprobePath = "")
{
    public bool HasExistingTools => File.Exists(FfmpegPath) && File.Exists(FfprobePath);
}

public sealed class ToolSettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private readonly string _settingsPath;

    public ToolSettingsService()
    {
        var settingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DVD26");
        _settingsPath = Path.Combine(settingsDirectory, "settings.json");
    }

    public ToolSettings Load()
    {
        try
        {
            return File.Exists(_settingsPath)
                ? JsonSerializer.Deserialize<ToolSettings>(File.ReadAllText(_settingsPath)) ?? new()
                : new();
        }
        catch (JsonException)
        {
            return new();
        }
    }

    public void Save(ToolSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!);
        File.WriteAllText(_settingsPath, JsonSerializer.Serialize(settings, JsonOptions));
    }
}
