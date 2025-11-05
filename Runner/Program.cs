using PbToDotNetCore;
using System.Reflection;

var testResults = new List<(string Category, string TestName, bool Success, string Error)>();
var totalTests = 0;
var passedTests = 0;
var failedTests = 0;

// Test runner function
void RunTest(string category, string testName, string code)
{
    totalTests++;
    Console.WriteLine($"\n=== {category}: {testName} ===");
    try
    {
        string result = PbToCsConverter.GenerateCsCode(code);
        Console.WriteLine("✅ PASSED - Generated C# code successfully");
        Console.WriteLine("Generated code preview (first 5 lines):");
        IEnumerable<string> lines = result.Split('\n').Take(5);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
                Console.WriteLine($"  {line}");
        }
        testResults.Add((category, testName, true, ""));
        passedTests++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ FAILED: {ex.Message}");
        testResults.Add((category, testName, false, ex.Message));
        failedTests++;
    }
}

Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║     PowerBASIC to C# Converter - Comprehensive Test Suite         ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

// 0. Test ENUM statements (NEW!)
Console.WriteLine("\n\n📁 CATEGORY: ENUM DECLARATIONS");
Console.WriteLine("─────────────────────────────────");
RunTest("ENUM", "Simple ENUM", Runner.Examples.EnumExamples.SimpleEnum);
RunTest("ENUM", "ENUM with Values", Runner.Examples.EnumExamples.EnumWithValues);
RunTest("ENUM", "ENUM Mixed Values", Runner.Examples.EnumExamples.EnumMixedValues);
RunTest("ENUM", "ENUM with Visibility", Runner.Examples.EnumExamples.EnumWithVisibility);

// 1. Test EXIT statements (Control Flow)
Console.WriteLine("\n\n📁 CATEGORY: EXIT STATEMENTS");
Console.WriteLine("─────────────────────────────────");
RunTest("EXIT Statements", "EXIT FOR", Runner.Examples.ExitExamples.ExitFor);
RunTest("EXIT Statements", "EXIT FUNCTION", Runner.Examples.ExitExamples.ExitFunction);
RunTest("EXIT Statements", "EXIT SUB", Runner.Examples.ExitExamples.ExitSub);
RunTest("EXIT Statements", "EXIT DO", Runner.Examples.ExitExamples.ExitDo);
RunTest("EXIT Statements", "Function Assignment", Runner.Examples.ExitExamples.FunctionAssignment);

// 2. Test CONST statements
Console.WriteLine("\n\n📁 CATEGORY: CONSTANTS");
Console.WriteLine("─────────────────────────────────");
RunTest("Constants", "Simple CONST", Runner.Examples.ConstAndWhileExamples.SimpleConst);
RunTest("Constants", "CONST with Types", Runner.Examples.ConstAndWhileExamples.ConstWithTypes);
RunTest("Constants", "CONST with Visibility", Runner.Examples.ConstAndWhileExamples.ConstWithVisibility);

// 3. Test WHILE...WEND loops
Console.WriteLine("\n\n📁 CATEGORY: WHILE...WEND LOOPS");
Console.WriteLine("─────────────────────────────────");
RunTest("WHILE Loops", "Simple WHILE...WEND", Runner.Examples.ConstAndWhileExamples.SimpleWhileWend);
RunTest("WHILE Loops", "WHILE with Condition", Runner.Examples.ConstAndWhileExamples.WhileWendWithCondition);
RunTest("WHILE Loops", "WHILE with Break", Runner.Examples.ConstAndWhileExamples.WhileWendWithBreak);
RunTest("WHILE Loops", "CONST and WHILE Combined", Runner.Examples.ConstAndWhileExamples.ConstAndWhileCombined);

// 4. Test File I/O
Console.WriteLine("\n\n📁 CATEGORY: FILE I/O OPERATIONS");
Console.WriteLine("─────────────────────────────────");
RunTest("File I/O", "File Write", Runner.Examples.FileIOExamples.FileWriteExample);
RunTest("File I/O", "File Read", Runner.Examples.FileIOExamples.FileReadExample);
RunTest("File I/O", "File Append", Runner.Examples.FileIOExamples.FileAppendExample);
RunTest("File I/O", "Multiple Files", Runner.Examples.FileIOExamples.MultipleFilesExample);
RunTest("File I/O", "File Loop", Runner.Examples.FileIOExamples.FileLoopExample);
RunTest("File I/O", "File Input", Runner.Examples.FileIOExamples.FileInputExample);
RunTest("File I/O", "Close All Files", Runner.Examples.FileIOExamples.CloseAllExample);

// 5. Test Basic Examples (from original test suite)
Console.WriteLine("\n\n📁 CATEGORY: BASIC FEATURES");
Console.WriteLine("─────────────────────────────────");
RunTest("Basic", "Hello World", Runner.Examples.BasicExamples.HelloWorld);
RunTest("Basic", "Variables", Runner.Examples.BasicExamples.VariablesExample);
RunTest("Basic", "Function with Params", Runner.Examples.BasicExamples.FunctionWithParams);
RunTest("Basic", "SUB Example", Runner.Examples.BasicExamples.SubExample);
RunTest("Basic", "FOR Loop", Runner.Examples.BasicExamples.ForLoopExample);
RunTest("Basic", "IF THEN", Runner.Examples.BasicExamples.IfThenExample);
RunTest("Basic", "SELECT CASE", Runner.Examples.BasicExamples.SelectCaseExample);
RunTest("Basic", "Arrays", Runner.Examples.BasicExamples.ArrayExample);

// 6. Test DO...LOOP (already implemented)
Console.WriteLine("\n\n📁 CATEGORY: DO...LOOP STATEMENTS");
Console.WriteLine("─────────────────────────────────");
RunTest("DO Loops", "Simple DO...LOOP", Runner.Examples.ExitExamples.ExitDo); // Has DO loop with EXIT DO

// 7. Test Edge Cases
Console.WriteLine("\n\n📁 CATEGORY: EDGE CASES & COMPLEX SCENARIOS");
Console.WriteLine("─────────────────────────────────");
RunTest("Edge Cases", "Nested EXITs", Runner.Examples.EdgeCaseExamples.NestedExits);
RunTest("Edge Cases", "Multiple CONST Types", Runner.Examples.EdgeCaseExamples.MultipleConstTypes);
RunTest("Edge Cases", "Nested WHILE Loops", Runner.Examples.EdgeCaseExamples.NestedWhileLoops);
RunTest("Edge Cases", "Mixed Loop Types", Runner.Examples.EdgeCaseExamples.MixedLoops);
RunTest("Edge Cases", "CONST Calculations", Runner.Examples.EdgeCaseExamples.ConstCalculations);
RunTest("Edge Cases", "WHILE with Exits", Runner.Examples.EdgeCaseExamples.WhileWithExits);

// Print Test Summary
Console.WriteLine("\n\n");
Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║                          TEST SUMMARY                              ║");
Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
Console.WriteLine($"\n  Total Tests:  {totalTests}");
Console.WriteLine($"  ✅ Passed:    {passedTests}");
Console.WriteLine($"  ❌ Failed:    {failedTests}");
Console.WriteLine($"  Success Rate: {(totalTests > 0 ? (passedTests * 100.0 / totalTests).ToString("F1") : "0")}%");

// Show failed tests details if any
if (failedTests > 0)
{
    Console.WriteLine("\n\n📋 FAILED TEST DETAILS:");
    Console.WriteLine("─────────────────────────────────");
    foreach ((string category, string testName, bool success, string error) in testResults.Where(r => !r.Success))
    {
        Console.WriteLine($"  ❌ {category} - {testName}");
        Console.WriteLine($"     Error: {error}");
    }
}

// Show feature coverage summary
Console.WriteLine("\n\n📊 FEATURE COVERAGE:");
Console.WriteLine("─────────────────────────────────");
var featureCoverage = new Dictionary<string, (int total, int passed)>();
foreach ((string category, string _, bool success, string _) in testResults)
{
    if (!featureCoverage.ContainsKey(category))
        featureCoverage[category] = (0, 0);

    (int total, int passed) current = featureCoverage[category];
    featureCoverage[category] = (current.total + 1, current.passed + (success ? 1 : 0));
}

foreach (KeyValuePair<string, (int total, int passed)> feature in featureCoverage)
{
    string percentage = feature.Value.total > 0
        ? (feature.Value.passed * 100.0 / feature.Value.total).ToString("F0")
        : "0";
    string status = feature.Value.passed == feature.Value.total ? "✅" : "⚠️";
    Console.WriteLine($"  {status} {feature.Key}: {feature.Value.passed}/{feature.Value.total} ({percentage}%)");
}

Console.WriteLine("\n\n✨ Test suite completed!");