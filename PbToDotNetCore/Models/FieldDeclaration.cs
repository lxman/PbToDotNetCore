namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a class field declaration
/// </summary>
public class FieldDeclaration
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "object";
    public string Visibility { get; set; } = "private";
}
