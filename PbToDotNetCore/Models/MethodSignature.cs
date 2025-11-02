namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a method signature (for interfaces)
/// </summary>
public class MethodSignature
{
    public string Name { get; set; } = string.Empty;
    public string ReturnType { get; set; } = "void";
    public List<ParameterDeclaration> Parameters { get; set; } = [];
}
