namespace PbToDotNetCore.Models;

/// <summary>
/// Represents an interface declaration
/// </summary>
public class InterfaceDeclaration
{
    public string Name { get; set; } = string.Empty;
    public List<string> InheritedInterfaces { get; set; } = [];
    public List<MethodSignature> Methods { get; set; } = [];
    public string Visibility { get; set; } = "public";
}
