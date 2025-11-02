namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a TYPE declaration (struct in C#)
/// </summary>
public class TypeDeclaration
{
    public string Name { get; set; } = string.Empty;
    public List<FieldDeclaration> Fields { get; set; } = [];
}
