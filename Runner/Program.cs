using PbToDotNetCore;

// Test all EXIT statements
Console.WriteLine("=== EXIT FOR Example ===");
try
{
    string result = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitFor);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("\n=== EXIT FUNCTION Example ===");
try
{
    string result = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitFunction);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("\n=== EXIT SUB Example ===");
try
{
    string result = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitSub);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("\n=== EXIT DO Example ===");
try
{
    string result = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitDo);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
