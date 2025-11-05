namespace PbToDotNetCore.Tests;

public class WhileWendExamples
{
    [Fact]
    public void TestSimpleWhileWend()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.SimpleWhileWend);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int counter;
                                        counter = 0;
                                        while (counter < 5)
                                        {
                                            MessageBox.Show("Counter: " + counter.ToString());
                                            counter++;
                                        }
                                        MessageBox.Show("Done!");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestWhileWendWithCondition()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.WhileWendWithCondition);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int x;
                                        int sum;
                                        x = 1;
                                        sum = 0;
                                        while (x <= 10)
                                        {
                                            sum = sum + x;
                                            x = x + 1;
                                        }
                                        MessageBox.Show("Sum of 1 to 10: " + sum.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestWhileWendWithBreak()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.WhileWendWithBreak);
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
                                        while (i < 100)
                                        {
                                            if (i == 42)
                                            {
                                                return PBMAIN_result;
                                            }
                                            i++;
                                        }
                                        MessageBox.Show("This should not appear");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestConstAndWhileCombined()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.ConstAndWhileCombined);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    const int MAX_ITERATIONS = 10;
                                    const int INCREMENT_VALUE = 2;
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int counter;
                                        int result;
                                        counter = 0;
                                        result = 0;
                                        while (counter < MAX_ITERATIONS)
                                        {
                                            result = result + INCREMENT_VALUE;
                                            counter++;
                                        }
                                        MessageBox.Show("Result: " + result.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestNestedWhileLoops()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.ConstAndWhileExamples.NestedWhileLoops);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int i;
                                        int j;
                                        int count;
                                        i = 0;
                                        while (i < 3)
                                        {
                                            j = 0;
                                            while (j < 2)
                                            {
                                                count = count + 1;
                                                j = j + 1;
                                            }
                                            i = i + 1;
                                        }
                                        MessageBox.Show("Count: " + count.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}