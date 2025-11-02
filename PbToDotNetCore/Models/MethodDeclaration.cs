namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a method implementation (with body)
/// </summary>
public class MethodDeclaration
{
    public string Name { get; set; } = string.Empty;
    public string ReturnType { get; set; } = "void";
    public List<ParameterDeclaration> Parameters { get; set; } = [];
    public List<Statement> Body { get; set; } = [];
    public string Visibility { get; set; } = "public";
}
