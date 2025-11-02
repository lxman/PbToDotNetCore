using PbToDotNetCore;

string csCode = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.BitOperations);

Console.WriteLine(csCode);
