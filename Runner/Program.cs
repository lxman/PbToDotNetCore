using PbToDotNetCore;

string csCode = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.FunctionAssignment);

Console.WriteLine(csCode);
