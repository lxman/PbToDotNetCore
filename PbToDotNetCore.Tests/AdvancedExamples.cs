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
                                    public class MyClass
                                    {
                                        public interface MyInterface : IUNKNOWN
                                        {
                                            int SayHello();
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