using PbToDotNetCore;
using Runner.Examples;

string csCode = PbToCsConverter.GenerateCsCode(AdvancedExamples.ClassWithInstanceVars);

Console.WriteLine(csCode);
