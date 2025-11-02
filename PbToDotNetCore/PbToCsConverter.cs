using Antlr4.Runtime;
using PbToDotNetCore.Parser;

namespace PbToDotNetCore;

public class PbToCsConverter
{
    public static string GenerateCsCode(string pbCode)
    {
        // Check if code contains MACRO - these are not supported
        if (ContainsMacro(pbCode))
        {
            return GenerateMacroUnsupportedMessage();
        }

        AntlrInputStream inputStream = CreateInputStream(pbCode);
        PowerBasicLexer lexer = CreateLexer(inputStream, new ConsoleErrorListener<int>());
        CommonTokenStream tokenStream = CreateTokenStream(lexer);
        PowerBasicParser parser = CreateParser(tokenStream, new ConsoleErrorListener<IToken>());
        PowerBasicParser.StartRuleContext? parseTree = GetParseTree(parser);
        var converter  = new PbToCSharpConverter();
        return converter.Visit(parseTree);
    }

    private static bool ContainsMacro(string pbCode)
    {
        // Simple check for MACRO keyword (case-insensitive)
        // Look for MACRO as a whole word
        return System.Text.RegularExpressions.Regex.IsMatch(
            pbCode,
            @"\bMACRO\b",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }

    private static string GenerateMacroUnsupportedMessage()
    {
        return """
            // ================================================================
            // CONVERSION FAILED: MACRO DETECTED
            // ================================================================
            // This PowerBASIC file contains one or more MACRO definitions.
            // PowerBASIC macros are compile-time text substitution and cannot
            // be automatically converted to C#.
            //
            // TO CONVERT THIS FILE:
            // 1. Identify all MACRO definitions in your PowerBASIC code
            // 2. Manually convert them to appropriate C# equivalents:
            //    - Simple value substitutions → const or static readonly
            //    - Expression macros → static methods or inline expressions
            //    - Complex macros → regular methods
            // 3. Replace all MACRO invocations with the C# equivalent
            // 4. Remove all MACRO definitions from the PowerBASIC file
            // 5. Run the conversion again
            //
            // EXAMPLE:
            // PowerBASIC:
            //   MACRO Square(x)
            //     ((x) * (x))
            //   END MACRO
            //
            // C# equivalents:
            //   private static int Square(int x) => x * x;
            //   // or inline: result = side * side;
            // ================================================================
            """;
    }

    public static AntlrInputStream CreateInputStream(string pbCode)
    {
        return new AntlrInputStream(pbCode);
    }

    public static PowerBasicLexer CreateLexer(AntlrInputStream inputStream, IAntlrErrorListener<int> errorListener)
    {
        var lexer = new PowerBasicLexer(inputStream);
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(errorListener);
        return lexer;
    }

    public static CommonTokenStream CreateTokenStream(PowerBasicLexer inputLexer)
    {
        var tokenStream = new CommonTokenStream(inputLexer);
        tokenStream.Fill();
        return tokenStream;
    }

    public static PowerBasicParser CreateParser(CommonTokenStream tokenStream, IAntlrErrorListener<IToken> errorListener)
    {
        var parser = new PowerBasicParser(tokenStream);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errorListener);
        return parser;
    }

    public static PowerBasicParser.StartRuleContext? GetParseTree(PowerBasicParser parser)
    {
        return parser.startRule();
    }
}