namespace PbToDotNetCore.Tests;

public class AdvancedExamples
{
    [Fact]
    public void TestSimpleClass()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.SimpleClass);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public interface MyInterface
                                    {
                                        int SayHello();
                                    }

                                    public class MyClass : MyInterface
                                    {
                                        public int SayHello()
                                        {
                                            MessageBox.Show("Hello from MyClass!");
                                            return 0;
                                        }

                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        MyInterface obj;
                                        obj = new MyClass();
                                        obj.SayHello();
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}