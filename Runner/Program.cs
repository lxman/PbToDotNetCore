using Antlr4.Runtime;
using PbToDotNetCore;
using PbToDotNetCore.Parser;
using Runner.Examples;

// Create input stream from source code
var inputStream = new AntlrInputStream(BasicExamples.VariablesExample);

// Create lexer (tokenizer)
var lexer = new PowerBasicLexer(inputStream);

// Remove default error listeners
lexer.RemoveErrorListeners();

// Add custom error listener to see lexer errors
lexer.AddErrorListener(new ConsoleErrorListener<int>());

// Create token stream
var tokenStream = new CommonTokenStream(lexer);

// Debug: Print all tokens
Console.WriteLine("=== TOKENS ===");
tokenStream.Fill();
foreach (IToken? token in tokenStream.GetTokens())
{
    if (token.Type == PowerBasicLexer.Eof) break;
    string tokenName = PowerBasicLexer.DefaultVocabulary.GetSymbolicName(token.Type) ?? "UNKNOWN";
    Console.WriteLine($"{tokenName}: '{token.Text}'");
}
tokenStream.Reset();
Console.WriteLine("=== END TOKENS ===\n");

// Create parser
var parser = new PowerBasicParser(tokenStream);

// Remove default error listeners
parser.RemoveErrorListeners();

// Add custom error listener to see detailed errors
parser.AddErrorListener(new ConsoleErrorListener<IToken>());

// Parse starting from the root rule
PowerBasicParser.StartRuleContext? parseTree = parser.startRule();

// Display parse tree as string
Console.WriteLine(parseTree.ToStringTree(parser));

// Usage:
var converter = new PbToCSharpConverter();
string? csharpCode = converter.Visit(parseTree);

Console.WriteLine(csharpCode);