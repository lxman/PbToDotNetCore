using PbToDotNetCore.Gui.WinForms;
using PbToDotNetCore.Gui.Wpf;

namespace PbToDotNetCore.Gui;

/// <summary>
/// Factory for creating the appropriate GUI converter based on global configuration
/// </summary>
public static class GuiConverterFactory
{
    private static IGuiConverter? _cachedConverter;
    private static GuiFramework _cachedFramework = GuiFramework.None;

    /// <summary>
    /// Gets the GUI converter for the currently configured target framework
    /// </summary>
    public static IGuiConverter GetConverter()
    {
        // Cache the converter instance to avoid creating new instances repeatedly
        if (_cachedConverter != null && _cachedFramework == GuiConfiguration.TargetFramework)
        {
            return _cachedConverter;
        }

        _cachedFramework = GuiConfiguration.TargetFramework;
        _cachedConverter = GuiConfiguration.TargetFramework switch
        {
            GuiFramework.WinForms => new WinFormsGuiConverter(),
            GuiFramework.Wpf => new WpfGuiConverter(),
            GuiFramework.None => new NoGuiConverter(),
            _ => throw new ArgumentException($"Unknown GUI framework: {GuiConfiguration.TargetFramework}")
        };

        return _cachedConverter;
    }

    /// <summary>
    /// Clears the cached converter instance. Call this if you change GuiConfiguration.TargetFramework.
    /// </summary>
    public static void ClearCache()
    {
        _cachedConverter = null;
    }
}
