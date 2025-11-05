namespace PbToDotNetCore.Tests;

public class RedimExamples
{
    [Fact]
    public void TestSimpleRedim()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.SimpleRedim);

        // Verify array resize without preserve
        Assert.Contains("myArray = new int[10 + 1]", actual);
    }

    [Fact]
    public void TestRedimPreserve()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.RedimPreserve);

        // Verify REDIM PRESERVE generates Array.Resize
        Assert.Contains("// REDIM PRESERVE numbers", actual);
        Assert.Contains("Array.Resize(ref numbers, 6 + 1)", actual);
    }

    [Fact]
    public void TestRedimDifferentTypes()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.RedimDifferentTypes);

        // Verify multiple REDIM PRESERVE statements
        Assert.Contains("Array.Resize(ref names", actual);
        Assert.Contains("Array.Resize(ref values", actual);
    }

    [Fact]
    public void TestRedimInLoop()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.RedimInLoop);

        // Verify REDIM inside loop
        Assert.Contains("for (int size = 1", actual);
        Assert.Contains("data = new int[", actual);
    }

    [Fact]
    public void TestDynamicGrowth()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.DynamicGrowth);

        // Verify dynamic array growth pattern
        Assert.Contains("items = new int[0 + 1]", actual);
        Assert.Contains("Array.Resize(ref items", actual);
    }

    [Fact]
    public void TestMultiDimensionalRedim()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.MultiDimensionalRedim);

        // Verify multi-dimensional array REDIM
        Assert.Contains("matrix = new int[3 + 1", actual);
    }

    [Fact]
    public void TestRedimWithType()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.RedimWithType);

        // Verify REDIM with AS type clause
        Assert.Contains("buffer = new byte[255 + 1]", actual);
        Assert.Contains("buffer = new byte[511 + 1]", actual);
    }

    [Fact]
    public void TestRedimMultiple()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.RedimExamples.RedimMultiple);

        // Verify multiple arrays in one REDIM statement
        Assert.Contains("arr1 = new int[5 + 1]", actual);
        Assert.Contains("arr2 = new int[10 + 1]", actual);
        Assert.Contains("arr3 = new int[15 + 1]", actual);
    }
}