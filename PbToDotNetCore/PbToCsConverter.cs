using Antlr4.Runtime;
using PbToDotNetCore.Parser;

namespace PbToDotNetCore;

public class PbToCsConverter
{
    public static string GenerateCsCode(string pbCode)
    {
        AntlrInputStream inputStream = CreateInputStream(pbCode);
        PowerBasicLexer lexer = CreateLexer(inputStream, new ConsoleErrorListener<int>());
        CommonTokenStream tokenStream = CreateTokenStream(lexer);
        PowerBasicParser parser = CreateParser(tokenStream, new ConsoleErrorListener<IToken>());
        PowerBasicParser.StartRuleContext? parseTree = GetParseTree(parser);
        var converter  = new PbToCSharpConverter();
        return converter.Visit(parseTree);
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