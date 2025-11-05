namespace PbToDotNetCore.Tests;

public class ErrorHandlingExamples
{
    [Fact]
    public void TestOnErrorGoto()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.OnErrorGoto);

        // Verify it generates try block and error handling comment
        Assert.Contains("try", actual);
        Assert.Contains("// Error handler will jump to ErrorHandler label", actual);
        // Note: Full label-based error handling is not yet implemented
        // This would require converting PowerBASIC labels to C# catch blocks
    }

    [Fact]
    public void TestOnErrorResumeNext()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.OnErrorResumeNext);

        // Verify it generates comment about ignoring errors
        Assert.Contains("// ON ERROR RESUME NEXT - errors will be silently ignored", actual);
    }

    [Fact]
    public void TestRaiseError()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.RaiseError);

        // Verify ERROR statement generates throw
        Assert.Contains("throw new Exception($\"Error {errorCode}\")", actual);
    }

    [Fact]
    public void TestResumeVariations()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.ResumeVariations);

        // Verify RESUME label generates goto
        Assert.Contains("goto TryAgain", actual);
    }

    [Fact]
    public void TestErrorInLoop()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.ErrorInLoop);

        // Verify ON ERROR RESUME NEXT comment is present
        Assert.Contains("// ON ERROR RESUME NEXT", actual);

        // Verify loop structure is maintained
        Assert.Contains("for (int i = -2", actual);
    }

    [Fact]
    public void TestMultipleErrorHandlers()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.MultipleErrorHandlers);

        // Verify function with error handling
        Assert.Contains("ProcessData", actual);
        Assert.Contains("throw new Exception", actual);
    }

    [Fact]
    public void TestNestedErrorHandling()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ErrorHandlingExamples.NestedErrorHandling);

        // Verify multiple functions with error handling
        Assert.Contains("InnerFunction", actual);
        Assert.Contains("OuterFunction", actual);
        Assert.Contains("try", actual);
    }
}