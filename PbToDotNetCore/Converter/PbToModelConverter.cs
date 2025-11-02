using PbToDotNetCore.Models;

namespace PbToDotNetCore.Converter;

/// <summary>
/// Converts PowerBASIC code to the intermediate representation (IR) model.
/// This is the foundation for multi-file project conversion.
/// </summary>
public class PbToModelConverter
{
    /// <summary>
    /// Convert a single PowerBASIC file to a CodeModel
    /// </summary>
    public static CodeModel ConvertFile(string pbCode, string? fileName = null)
    {
        // TODO: Implement conversion from ANTLR AST to CodeModel
        // For now, this is a placeholder showing the intended architecture
        throw new NotImplementedException(
            "PbToModelConverter is part of the future multi-file architecture. " +
            "Use PbToCsConverter.GenerateCsCode() for single-file conversion.");
    }

    /// <summary>
    /// Convert multiple PowerBASIC files to a unified CodeModel
    /// </summary>
    public static CodeModel ConvertProject(Dictionary<string, string> files)
    {
        // TODO: Parse all files, merge into single CodeModel, resolve cross-file references
        throw new NotImplementedException(
            "Multi-file project conversion is not yet implemented. " +
            "This is the intended future architecture.");
    }
}
