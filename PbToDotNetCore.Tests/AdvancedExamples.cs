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
    
    [Fact]
    public void TestClassWithInstanceVars()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.ClassWithInstanceVars);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public interface ICounter
                                    {
                                        int Increment();
                                        int GetValue();
                                        void Reset();
                                    }
                                
                                    public class Counter : ICounter
                                    {
                                        private int Count;
                                
                                        public int Increment()
                                        {
                                            Count++;
                                            return Count;
                                        }
                                
                                        public int GetValue()
                                        {
                                            return Count;
                                        }
                                
                                        public void Reset()
                                        {
                                            Count = 0;
                                        }
                                
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        ICounter counter;
                                        int value;
                                        counter = new Counter();
                                        counter.Increment();
                                        counter.Increment();
                                        counter.Increment();
                                        value = counter.GetValue();
                                        MessageBox.Show("Count: " + value.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestMultipleInterfaces()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.MultipleInterfaces);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public interface IMath
                                    {
                                        double Add(double a, double b);
                                        double Multiply(double a, double b);
                                    }
                                
                                    public interface IHistory
                                    {
                                        double GetLastResult();
                                    }
                                
                                    public class Calculator : IMath, IHistory
                                    {
                                        private double LastResult;
                                
                                        public double Add(double a, double b)
                                        {
                                            LastResult = a + b;
                                            return LastResult;
                                        }
                                
                                        public double Multiply(double a, double b)
                                        {
                                            LastResult = a * b;
                                            return LastResult;
                                        }
                                
                                        public double GetLastResult()
                                        {
                                            return LastResult;
                                        }
                                
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        IMath math;
                                        IHistory history;
                                        double result;
                                        math = new Calculator();
                                        result = math.Add(10, 5);
                                        history = math;
                                        result = history.GetLastResult();
                                        MessageBox.Show("Last result: " + result.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestClassWithUnion()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.ClassWithUnion);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
                                    public struct DataUnion
                                    {
                                        [System.Runtime.InteropServices.FieldOffset(0)]
                                        public int LongValue;
                                        [System.Runtime.InteropServices.FieldOffset(0)]
                                        public uint DWordValue;
                                        [System.Runtime.InteropServices.FieldOffset(0)]
                                        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4)]
                                        public byte[] ByteArray;
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        DataUnion data;
                                        data.LongValue = -1;
                                        MessageBox.Show("LONG: " + data.LongValue.ToString() + Environment.NewLine + "DWORD: " + data.DWordValue.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestInlineAssembly()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.InlineAssembly);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        int result;
                                        // ============================================================
                                        // INLINE ASSEMBLY DETECTED - MANUAL CONVERSION REQUIRED
                                        // PowerBASIC inline assembly cannot be automatically converted.
                                        // Original assembly code:
                                        //   ! MOV EAX, 100
                                        //   ! ADD EAX, 50
                                        //   ! MOV result, EAX
                                        // Consider: C# arithmetic operations, System.Runtime.Intrinsics, or P/Invoke
                                        // ============================================================
                                        throw new NotImplementedException("Inline assembly block with 3 instruction(s)");
                                        MessageBox.Show("Assembly result: " + result.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestPointerExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.PointerExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        int value;
                                        // ============================================================
                                        // POINTER TYPE DETECTED - MANUAL CONVERSION REQUIRED
                                        // PowerBASIC pointer: pValue AS AS LONG PTR
                                        // Consider: IntPtr, unsafe pointers, or Span<T>/Memory<T>
                                        // ============================================================
                                        IntPtr pValue; // TODO: Convert pointer operations
                                        value = 42;
                                        pValue = IntPtr.Zero /* TODO: VARPTR(value) - Consider Marshal.AllocHGlobal or GCHandle.Alloc */;
                                        MessageBox.Show();
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
    
    [Fact]
    public void TestMacroExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.MacroExample);
        const string expected = """
                                // ================================================================
                                // CONVERSION FAILED: MACRO DETECTED
                                // ================================================================
                                // This PowerBASIC file contains one or more MACRO definitions.
                                // PowerBASIC macros are compile-time text substitution and cannot
                                // be automatically converted to C#.
                                //
                                // TO CONVERT THIS FILE:
                                // 1. Identify all MACRO definitions in your PowerBASIC code
                                // 2. Manually convert them to appropriate C# equivalents:
                                //    - Simple value substitutions → const or static readonly
                                //    - Expression macros → static methods or inline expressions
                                //    - Complex macros → regular methods
                                // 3. Replace all MACRO invocations with the C# equivalent
                                // 4. Remove all MACRO definitions from the PowerBASIC file
                                // 5. Run the conversion again
                                //
                                // EXAMPLE:
                                // PowerBASIC:
                                //   MACRO Square(x)
                                //     ((x) * (x))
                                //   END MACRO
                                //
                                // C# equivalents:
                                //   private static int Square(int x) => x * x;
                                //   // or inline: result = side * side;
                                // ================================================================
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestThreadExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.ThreadExample);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int WorkerThread(uint param)
                                    {
                                        int WorkerThread_result;
                                        for (int i = 1; i <= 5; i += 1)
                                        {
                                            SLEEP(500);
                                        }
                                        FUNCTION = 0;
                                        return WorkerThread_result;
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        uint hThread;
                                        // ============================================================
                                        // THREAD BLOCK DETECTED - MANUAL CONVERSION REQUIRED
                                        // PowerBASIC THREAD CREATE...CLOSE block with 5 statement(s)
                                        //
                                        // Original PowerBASIC code:
                                        //   THREAD CREATE WorkerThread TO hThread
                                        //   MSGBOX "Thread started"
                                        //   THREAD WAIT hThread
                                        //   MSGBOX "Thread completed"
                                        //   THREAD CLOSE hThread
                                        //
                                        // Conversion Guide:
                                        // - Replace THREAD CREATE func TO handle with: thread = new Thread(() => func()); thread.Start();
                                        // - Replace THREAD WAIT handle with: thread.Join();
                                        // - Remove THREAD CLOSE handle (automatic cleanup in C#)
                                        // - Convert other statements within the block as needed
                                        // ============================================================
                                        throw new NotImplementedException("Thread block requires manual conversion");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestBitOperations()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.BitOperations);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result;
                                        uint value;
                                        uint shifted;
                                        uint rotated;
                                        value = 0b00001111;
                                        shifted = value;
                                        shifted <<= 2;
                                        rotated = value;
                                        rotated = (rotated << 2) | (rotated >> (32 - 2));
                                        MessageBox.Show("Original: " + Convert.ToString(value, 2) + Environment.NewLine + "Shifted left 2: " + Convert.ToString(shifted, 2) + Environment.NewLine + "Rotated left 2: " + Convert.ToString(rotated, 2));
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestSourceCode1()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.AdvancedExamples.SourceCode1);
        const string expected = """
                                using System;
                                using System.Windows.Forms;
                                
                                public class PowerBasicModule
                                {
                                    public interface IStatus
                                    {
                                        void Done();
                                    }
                                
                                    public class EvClass : IStatus
                                    {
                                        public void Done()
                                        {
                                            Console.WriteLine("Done!");
                                        }
                                
                                    }
                                    public interface IMath
                                    {
                                        void DoMath();
                                    }
                                
                                    public class MyClass : IMath
                                    {
                                        public void DoMath()
                                        {
                                            Console.WriteLine("Calculating...");
                                        }
                                
                                    }
                                    public object PBMAIN()
                                    {
                                        object PBMAIN_result;
                                        IMath oMath;
                                        IStatus oStatus;
                                        oMath = new MyClass();
                                        oStatus = new EvClass();
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}