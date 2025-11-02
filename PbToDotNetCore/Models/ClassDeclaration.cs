namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a class declaration
/// </summary>
public class ClassDeclaration
{
    public string Name { get; set; } = string.Empty;
    public List<string> ImplementedInterfaces { get; set; } = [];
    public List<FieldDeclaration> Fields { get; set; } = [];
    public List<MethodDeclaration> Methods { get; set; } = [];
    public string Visibility { get; set; } = "public";
}
