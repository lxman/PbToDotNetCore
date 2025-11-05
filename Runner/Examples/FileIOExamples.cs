namespace Runner.Examples;

public static class FileIOExamples
{
    // Basic file write example
    public const string FileWriteExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING
            LOCAL textLine AS STRING

            fileName = "test.txt"
            textLine = "Hello, File!"

            OPEN fileName FOR OUTPUT AS #1
            PRINT #1, textLine
            PRINT #1, "Second line"
            CLOSE #1

            MSGBOX "File written successfully"
        END FUNCTION
        """;

    // Basic file read example
    public const string FileReadExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING
            LOCAL textLine AS STRING

            fileName = "test.txt"

            OPEN fileName FOR INPUT AS #1
            LINE INPUT #1, textLine
            CLOSE #1

            MSGBOX "Read: " & textLine
        END FUNCTION
        """;

    // Append to file example
    public const string FileAppendExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING

            fileName = "log.txt"

            OPEN fileName FOR APPEND AS #2
            PRINT #2, "Log entry: " & TIME$
            PRINT #2, "User action recorded"
            CLOSE #2

            MSGBOX "Log appended"
        END FUNCTION
        """;

    // Multiple file handles example
    public const string MultipleFilesExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL sourceFile AS STRING
            LOCAL destFile AS STRING
            LOCAL textLine AS STRING

            sourceFile = "input.txt"
            destFile = "output.txt"

            OPEN sourceFile FOR INPUT AS #1
            OPEN destFile FOR OUTPUT AS #2

            LINE INPUT #1, textLine
            PRINT #2, "Processed: " & textLine

            CLOSE #1
            CLOSE #2

            MSGBOX "Files processed"
        END FUNCTION
        """;

    // File I/O with loop example
    public const string FileLoopExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING
            LOCAL i AS LONG

            fileName = "numbers.txt"

            OPEN fileName FOR OUTPUT AS #1
            FOR i = 1 TO 10
                PRINT #1, "Number: " & FORMAT$(i)
            NEXT i
            CLOSE #1

            MSGBOX "Numbers written to file"
        END FUNCTION
        """;

    // Input from file example
    public const string FileInputExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING
            LOCAL name AS STRING
            LOCAL age AS LONG

            fileName = "data.txt"

            OPEN fileName FOR OUTPUT AS #1
            PRINT #1, "John Doe"
            PRINT #1, "30"
            CLOSE #1

            OPEN fileName FOR INPUT AS #1
            INPUT #1, name
            INPUT #1, age
            CLOSE #1

            MSGBOX "Name: " & name & ", Age: " & FORMAT$(age)
        END FUNCTION
        """;

    // Close all files example
    public const string CloseAllExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            OPEN "file1.txt" FOR OUTPUT AS #1
            OPEN "file2.txt" FOR OUTPUT AS #2
            OPEN "file3.txt" FOR OUTPUT AS #3

            PRINT #1, "File 1 content"
            PRINT #2, "File 2 content"
            PRINT #3, "File 3 content"

            CLOSE  ' Close all open files

            MSGBOX "All files closed"
        END FUNCTION
        """;

    // Simple file I/O - write then read
    public const string SimpleFileIO = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            OPEN "test.txt" FOR OUTPUT AS #1
            PRINT #1, "Test"
            CLOSE #1

            OPEN "test.txt" FOR INPUT AS #1
            LOCAL content AS STRING
            LINE INPUT #1, content
            CLOSE #1

            MSGBOX content
        END FUNCTION
        """;
}