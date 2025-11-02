namespace PbToDotNetCore.Models;

/// <summary>
/// Represents a parameter declaration
/// </summary>
public class ParameterDeclaration
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "object";
    public bool IsByRef { get; set; } = false;
}
