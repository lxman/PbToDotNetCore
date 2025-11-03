namespace PbToDotNetCore.Tests;

public class ExitExamples
{
    [Fact]
    public void TestExitFor()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitFor);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        for (int i = 1; i <= 100; i += 1)
                                        {
                                            if (i == 42)
                                            {
                                                MessageBox.Show("Found 42!");
                                                break;
                                            }
                                        }
                                        MessageBox.Show("Done");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestExitFunction()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitFunction);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int ValidateAge(int age)
                                    {
                                        int ValidateAge_result = default;
                                        if (age < 0)
                                        {
                                            return ValidateAge_result;
                                        }
                                        return ValidateAge_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestFunctionAssignment()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.FunctionAssignment);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int GetValue()
                                    {
                                        int GetValue_result = default;
                                        FUNCTION = 42;
                                        return GetValue_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestExitSub()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitSub);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public void ProcessValue(int value)
                                    {
                                        if (value == 0)
                                        {
                                            return;
                                        }
                                        MessageBox.Show("Processing: " + value.ToString());
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        ProcessValue(0);
                                        ProcessValue(10);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}