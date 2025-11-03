using System.Text.Json;
using System.Text.Json.Serialization;

namespace PbToDotNetCore.Gui;

/// <summary>
/// Configuration file structure for PbToDotNetCore settings
/// </summary>
internal class PbToDotNetCoreConfig
{
    [JsonPropertyName("GuiFramework")]
    public string GuiFramework { get; set; } = "None";
}

/// <summary>
/// Loads configuration from pbtodotnetcore.config.json
/// </summary>
public static class ConfigurationLoader
{
    private static bool _configLoaded = false;
    private const string ConfigFileName = "pbtodotnetcore.config.json";

    /// <summary>
    /// Loads configuration from the config file and applies it to GuiConfiguration.
    /// Searches for the config file in:
    /// 1. Current working directory
    /// 2. Application base directory
    /// 3. User's home directory
    /// </summary>
    public static void LoadConfiguration()
    {
        if (_configLoaded)
        {
            return;
        }

        string? configPath = FindConfigFile();

        if (configPath != null && File.Exists(configPath))
        {
            try
            {
                string json = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<PbToDotNetCoreConfig>(json);

                if (config != null && !string.IsNullOrWhiteSpace(config.GuiFramework))
                {
                    if (Enum.TryParse<GuiFramework>(config.GuiFramework, true, out var framework))
                    {
                        GuiConfiguration.TargetFramework = framework;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Invalid GuiFramework value '{config.GuiFramework}' in {ConfigFileName}. Using default (None).");
                    }
                }

                _configLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load configuration from {configPath}: {ex.Message}");
            }
        }
        else
        {
            // No config file found - use defaults
            _configLoaded = true;
        }
    }

    /// <summary>
    /// Reloads configuration from disk, even if already loaded
    /// </summary>
    public static void ReloadConfiguration()
    {
        _configLoaded = false;
        LoadConfiguration();
    }

    private static string? FindConfigFile()
    {
        // Search locations in priority order
        var searchPaths = new[]
        {
            // 1. Current working directory
            Path.Combine(Directory.GetCurrentDirectory(), ConfigFileName),

            // 2. Application base directory (where the DLL is located)
            Path.Combine(AppContext.BaseDirectory, ConfigFileName),

            // 3. User's home directory
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ConfigFileName)
        };

        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        return null;
    }

    /// <summary>
    /// Creates a sample configuration file in the specified directory
    /// </summary>
    /// <param name="directory">Target directory (defaults to current directory)</param>
    public static void CreateSampleConfig(string? directory = null)
    {
        directory ??= Directory.GetCurrentDirectory();
        string path = Path.Combine(directory, ConfigFileName);

        var sampleConfig = new
        {
            GuiFramework = "None",
            Comments = new
            {
                GuiFramework = "Specifies the target GUI framework for PowerBASIC GUI code conversion. Valid values: 'None', 'WinForms', 'Wpf'. Default: 'None'"
            }
        };

        string json = JsonSerializer.Serialize(sampleConfig, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(path, json);
        Console.WriteLine($"Sample configuration file created at: {path}");
    }
}
