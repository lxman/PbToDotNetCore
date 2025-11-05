namespace PbToDotNetCore.Tests;

public class ConstExamples
{
    [Fact]
    public void TestSimpleConst()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.SimpleConst);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    const double PI = 3.14159;
                                    const int MAX_COUNT = 100;
                                    const string MESSAGE = "Hello, World!";
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        MessageBox.Show(MESSAGE);
                                        MessageBox.Show(PI.ToString());
                                        MessageBox.Show(MAX_COUNT.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestConstWithTypes()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.ConstWithTypes);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    const double PI = 3.14159265359;
                                    const int MAX_SIZE = 1024;
                                    const string APP_NAME = "My Application";
                                    const int IS_DEBUG = 1;
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        MessageBox.Show(APP_NAME + " - Max Size: " + MAX_SIZE.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestConstWithVisibility()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.ConstWithVisibility);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public const string VERSION = "1.0.0";
                                    private const string SECRET_KEY = "XYZ123";
                                    public const int APP_ID = 42;
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        MessageBox.Show("Version: " + VERSION);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestConstMultipleDeclarations()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.ConstMultipleDeclarations);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    const int A = 1;
                                    const int B = 2;
                                    const int C = 3;
                                    const string X = "Test";
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        MessageBox.Show(A + B + C.ToString() + " " + X);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}