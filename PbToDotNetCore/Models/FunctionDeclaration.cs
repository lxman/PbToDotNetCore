namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a module-level function or sub declaration
/// </summary>
public class FunctionDeclaration
{
    public string Name { get; set; } = string.Empty;
    public string ReturnType { get; set; } = "void";
    public List<ParameterDeclaration> Parameters { get; set; } = [];
    public List<VariableDeclaration> LocalVariables { get; set; } = [];
    public List<Statement> Body { get; set; } = [];
    public string Visibility { get; set; } = "public";
    public bool IsStatic { get; set; } = false;
    public bool IsCallback { get; set; } = false;
}
