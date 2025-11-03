namespace PbToDotNetCore.Gui;

/// <summary>
/// Global configuration for GUI framework conversion
/// </summary>
public static class GuiConfiguration
{
    /// <summary>
    /// The target GUI framework for PowerBASIC GUI code conversion.
    /// This is a global setting - all GUI code in the project will be converted to the same framework.
    /// Default: GuiFramework.None (no automatic conversion)
    /// </summary>
    public static GuiFramework TargetFramework { get; set; } = GuiFramework.None;
}
