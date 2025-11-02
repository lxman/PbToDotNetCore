namespace PbToDotNetCore.Models;

/// <summary>
/// Top-level model representing a complete PowerBASIC module
/// </summary>
public class CodeModel
{
    public string ModuleName { get; set; } = "PowerBasicModule";
    public List<string> UsingDirectives { get; set; } =
    [
        "System",
        "System.Windows.Forms"
    ];
    public List<ClassDeclaration> Classes { get; set; } = [];
    public List<InterfaceDeclaration> Interfaces { get; set; } = [];
    public List<FunctionDeclaration> Functions { get; set; } = [];
    public List<TypeDeclaration> Types { get; set; } = [];
    public List<VariableDeclaration> GlobalVariables { get; set; } = [];
}
