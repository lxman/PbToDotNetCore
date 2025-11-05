namespace PbToDotNetCore.Tests;

public class DoLoopExamples
{
    [Fact]
    public void TestExitDo()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ExitExamples.ExitDo);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int counter;
                                        do
                                        {
                                            counter++;
                                            if (counter > 10)
                                            {
                                                break;
                                            }
                                        } while (true);
                                        MessageBox.Show("Counter: " + counter.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestSimpleDoLoop()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.DoLoopExamples.SimpleDoLoop);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int i;
                                        i = 0;
                                        do
                                        {
                                            i = i + 1;
                                        } while (true);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestDoWhileLoop()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.DoLoopExamples.DoWhileLoop);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int i;
                                        i = 0;
                                        while (i < 5)
                                        {
                                            i = i + 1;
                                        }
                                        MessageBox.Show(i.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestDoUntilLoop()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.DoLoopExamples.DoUntilLoop);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int i;
                                        i = 0;
                                        while (!(i >= 5))
                                        {
                                            i = i + 1;
                                        }
                                        MessageBox.Show(i.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}