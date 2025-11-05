namespace PbToDotNetCore.Tests;

public class EnumExamples
{
    [Fact]
    public void TestSimpleEnum()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.SimpleEnum);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum Colors
                                    {
                                        Red,
                                        Green,
                                        Blue,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int myColor;
                                        myColor = Green;
                                        MessageBox.Show("Color value: " + myColor.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestEnumWithValues()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.EnumWithValues);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum StatusCodes
                                    {
                                        OK = 200,
                                        NotFound = 404,
                                        ServerError = 500,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int status;
                                        status = NotFound;
                                        MessageBox.Show("Status code: " + status.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestEnumMixedValues()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.EnumMixedValues);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum Priority
                                    {
                                        Low = 1,
                                        Medium = 2,
                                        High = 3,
                                        Critical = 10,
                                        Emergency = 11,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int level;
                                        level = High;
                                        MessageBox.Show("Priority level: " + level.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestEnumWithVisibility()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.EnumWithVisibility);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum DaysOfWeek
                                    {
                                        Sunday,
                                        Monday,
                                        Tuesday,
                                        Wednesday,
                                        Thursday,
                                        Friday,
                                        Saturday,
                                    }
                                    private enum AccessLevel
                                    {
                                        None = 0,
                                        Read = 1,
                                        Write = 2,
                                        Execute = 4,
                                        FullAccess = 7,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int day;
                                        int access;
                                        day = Wednesday;
                                        access = FullAccess;
                                        MessageBox.Show("Day: " + day.ToString() + ", Access: " + access.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestEnumWithSelectCase()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.EnumWithSelectCase);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum FileMode
                                    {
                                        Read = 1,
                                        Write = 2,
                                        Append = 3,
                                        ReadWrite = 4,
                                    }
                                    public string GetModeString(int mode)
                                    {
                                        string GetModeString_result = default;
                                        switch (mode)
                                        {
                                            case Read:
                                                FUNCTION = "Read Only";
                                                break;
                                            case Write:
                                                FUNCTION = "Write Only";
                                                break;
                                            case Append:
                                                FUNCTION = "Append Mode";
                                                break;
                                            case ReadWrite:
                                                FUNCTION = "Read/Write";
                                                break;
                                            default:
                                                FUNCTION = "Unknown";
                                                break;
                                        }
                                        return GetModeString_result;
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int mode;
                                        mode = ReadWrite;
                                        MessageBox.Show(GetModeString(mode));
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestMultipleEnums()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.MultipleEnums);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum Color
                                    {
                                        Red = 1,
                                        Green = 2,
                                        Blue = 3,
                                    }
                                    public enum Size
                                    {
                                        Small = 10,
                                        Medium = 20,
                                        Large = 30,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int color;
                                        int size;
                                        color = Blue;
                                        size = Medium;
                                        MessageBox.Show("Color: " + color.ToString() + ", Size: " + size.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestEnumNonZeroStart()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.EnumExamples.EnumNonZeroStart);
        const string expected = """
                                using System;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public enum ErrorCodes
                                    {
                                        NoError = 100,
                                        FileNotFound = 101,
                                        AccessDenied = 102,
                                        InvalidParameter = 103,
                                        OutOfMemory = 104,
                                    }
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        int errorCode;
                                        errorCode = AccessDenied;
                                        MessageBox.Show("Error code: " + errorCode.ToString());
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}