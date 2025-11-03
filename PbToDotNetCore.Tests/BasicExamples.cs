namespace PbToDotNetCore.Tests;

public class BasicExamples
{
    [Fact]
    public void TestHelloWorld()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.HelloWorld);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                  public int PBMAIN()
                                  {
                                      int PBMAIN_result = default;
                                      MessageBox.Show("Hello World");
                                      return PBMAIN_result;
                                  }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestFunctionWithParams()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.FunctionWithParams);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int Add(int x, int y)
                                    {
                                        int Add_result = default;
                                        int result;
                                        result = x + y;
                                        Add_result = result;
                                        return Add_result;
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int answer;
                                        answer = Add(5, 10);
                                        MessageBox.Show("5 + 10 = " + answer.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestVariablesExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.VariablesExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string name;
                                        int age;
                                        double price;
                                        int flag;
                                        name = "PowerBASIC";
                                        age = 25;
                                        price = 99.95;
                                        flag = 1;
                                        MessageBox.Show("Name: " + name + Environment.NewLine + "Age: " + age.ToString() + Environment.NewLine + "Price: $" + price.ToString("#.00"));
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestSubExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.SubExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public void PrintMessage(string msg)
                                    {
                                        MessageBox.Show(msg);
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        PrintMessage("Hello from SUB");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestForLoopExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.ForLoopExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string result;
                                        for (int i = 1; i <= 10; i += 1)
                                        {
                                            result = result + i.ToString() + " ";
                                        }
                                        MessageBox.Show("Numbers 1-10: " + result);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestIfThenExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.IfThenExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int number;
                                        string msg;
                                        number = 42;
                                        if (number > 50)
                                        {
                                            msg = "Greater than 50";
                                        }
                                        else if (number > 25)
                                        {
                                            msg = "Between 25 and 50";
                                        }
                                        else
                                        {
                                            msg = "25 or less";
                                        }
                                        MessageBox.Show(msg);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestSelectCaseExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.SelectCaseExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int dayNum;
                                        string dayName;
                                        dayNum = 3;
                                        switch (dayNum)
                                        {
                                            case 1:
                                                dayName = "Monday";
                                                break;
                                            case 2:
                                                dayName = "Tuesday";
                                                break;
                                            case 3:
                                                dayName = "Wednesday";
                                                break;
                                            case 4:
                                                dayName = "Thursday";
                                                break;
                                            case 5:
                                                dayName = "Friday";
                                                break;
                                            default:
                                                dayName = "Weekend";
                                                break;
                                        }
                                        MessageBox.Show("Day " + dayNum.ToString() + " is " + dayName);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestArrayExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.ArrayExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int[] numbers = new int[10 + 1];
                                        int total;
                                        for (int i = 1; i <= 10; i += 1)
                                        {
                                            numbers[i] = i * 2;
                                            total = total + numbers[i];
                                        }
                                        MessageBox.Show("Sum of even numbers 2-20: " + total.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestSourceCode1()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.BasicExamples.SourceCode1);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public object RandomAdd()
                                    {
                                        object RandomAdd_result = default;
                                        return RandomAdd_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}