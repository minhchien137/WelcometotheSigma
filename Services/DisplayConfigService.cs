using System.Text.Json;
using WelcometotheSigma.Models;

namespace WelcometotheSigma.Services;

public class DisplayConfigService : IDisplayConfigService
{
    private static readonly JsonSerializerOptions _opts = new() { WriteIndented = true };
    private readonly string _filePath;
    private DisplayConfig _cached;
    private readonly Lock _lock = new();

    public DisplayConfigService(IWebHostEnvironment env)
    {
        var dir = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "display-config.json");
        _cached   = Load();
    }

    public DisplayConfig Get()
    {
        lock (_lock) return _cached;
    }

    public void Save(DisplayConfig config)
    {
        lock (_lock)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(config, _opts));
            _cached = config;
        }
    }

    private DisplayConfig Load()
    {
        if (!File.Exists(_filePath)) return new DisplayConfig();
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<DisplayConfig>(json) ?? new DisplayConfig();
        }
        catch { return new DisplayConfig(); }
    }
}
