namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a variable declaration
/// </summary>
public class VariableDeclaration
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "object";
    public bool IsArray { get; set; } = false;
    public string? ArraySize { get; set; }
}
