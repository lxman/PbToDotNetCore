namespace PbToDotNetCore.Tests;

public class FileIOExamples
{
    [Fact]
    public void TestFileWriteExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.FileWriteExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string fileName;
                                        string textLine;
                                        fileName = "test.txt";
                                        textLine = "Hello, File!";
                                        StreamWriter file1 = new StreamWriter(fileName, false);
                                        file1.WriteLine(textLine);
                                        file1.WriteLine("Second line");
                                        file1?.Close();
                                        MessageBox.Show("File written successfully");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestFileReadExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.FileReadExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string fileName;
                                        string textLine;
                                        fileName = "test.txt";
                                        StreamReader file1 = new StreamReader(fileName);
                                        textLine = file1.ReadLine();
                                        file1?.Close();
                                        MessageBox.Show("Read: " + textLine);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestFileAppendExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.FileAppendExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string fileName;
                                        fileName = "log.txt";
                                        StreamWriter file2 = new StreamWriter(fileName, true);
                                        file2.WriteLine("Log entry: " + TIME$);
                                        file2.WriteLine("User action recorded");
                                        file2?.Close();
                                        MessageBox.Show("Log appended");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestMultipleFilesExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.MultipleFilesExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string sourceFile;
                                        string destFile;
                                        string textLine;
                                        sourceFile = "input.txt";
                                        destFile = "output.txt";
                                        StreamReader file1 = new StreamReader(sourceFile);
                                        StreamWriter file2 = new StreamWriter(destFile, false);
                                        textLine = file1.ReadLine();
                                        file2.WriteLine("Processed: " + textLine);
                                        file1?.Close();
                                        file2?.Close();
                                        MessageBox.Show("Files processed");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestFileLoopExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.FileLoopExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        string fileName;
                                        fileName = "numbers.txt";
                                        StreamWriter file1 = new StreamWriter(fileName, false);
                                        for (int i = 1; i <= 10; i += 1)
                                        {
                                            file1.WriteLine("Number: " + i.ToString());
                                        }
                                        file1?.Close();
                                        MessageBox.Show("Numbers written to file");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestCloseAllExample()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.CloseAllExample);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        StreamWriter file1 = new StreamWriter("file1.txt", false);
                                        StreamWriter file2 = new StreamWriter("file2.txt", false);
                                        StreamWriter file3 = new StreamWriter("file3.txt", false);
                                        file1.WriteLine("File 1 content");
                                        file2.WriteLine("File 2 content");
                                        file3.WriteLine("File 3 content");
                                        file1?.Close();
                                        file2?.Close();
                                        file3?.Close();
                                        MessageBox.Show("All files closed");
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }

    [Fact]
    public void TestSimpleFileIO()
    {
        string actual = PbToCsConverter.GenerateCsCode(Runner.Examples.FileIOExamples.SimpleFileIO);
        const string expected = """
                                using System;
                                using System.IO;
                                using System.Windows.Forms;

                                public class PowerBasicModule
                                {
                                    public int PBMAIN()
                                    {
                                        int PBMAIN_result = default;
                                        StreamWriter file1 = new StreamWriter("test.txt", false);
                                        file1.WriteLine("Test");
                                        file1?.Close();
                                        StreamReader file1 = new StreamReader("test.txt");
                                        string content;
                                        content = file1.ReadLine();
                                        file1?.Close();
                                        MessageBox.Show(content);
                                        return PBMAIN_result;
                                    }
                                }
                                """;
        DiffCheck.Verify(expected, actual);
    }
}