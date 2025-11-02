using PbToDotNetCore;
using Runner.Examples;

string csCode = PbToCsConverter.GenerateCsCode(AdvancedExamples.SimpleClass);

Console.WriteLine(csCode);
