namespace PbToDotNetCore.Gui;

/// <summary>
/// Supported GUI frameworks for PowerBASIC to C# conversion
/// </summary>
public enum GuiFramework
{
    /// <summary>
    /// Windows Forms - Traditional .NET GUI framework
    /// </summary>
    WinForms,

    /// <summary>
    /// Windows Presentation Foundation - Modern XAML-based framework
    /// </summary>
    Wpf,

    /// <summary>
    /// No GUI conversion - mark GUI code as requiring manual conversion
    /// </summary>
    None
}
