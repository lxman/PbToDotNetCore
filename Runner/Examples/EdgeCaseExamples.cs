namespace Runner.Examples;

public static class EdgeCaseExamples
{
    // Nested EXIT statements
    public const string NestedExits = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL j AS LONG

            FOR i = 1 TO 10
                FOR j = 1 TO 10
                    IF i * j = 42 THEN
                        MSGBOX "Found 42!"
                        EXIT FOR
                    END IF
                NEXT j
                IF i = 7 THEN
                    EXIT FOR
                END IF
            NEXT i
        END FUNCTION
        """;

    // Multiple CONST declarations
    public const string MultipleConstTypes = """
        #COMPILE EXE
        #DIM ALL

        CONST A = 1, B = 2, C = 3
        CONST X AS STRING = "Hello", Y AS STRING = "World"
        CONST PI AS DOUBLE = 3.14159, E AS DOUBLE = 2.71828

        FUNCTION PBMAIN() AS LONG
            MSGBOX FORMAT$(A + B + C)
            MSGBOX X & " " & Y
            MSGBOX FORMAT$(PI * E)
        END FUNCTION
        """;

    // Nested WHILE loops
    public const string NestedWhileLoops = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL j AS LONG
            LOCAL sum AS LONG

            i = 0
            WHILE i < 3
                j = 0
                WHILE j < 3
                    sum = sum + (i * 10 + j)
                    j = j + 1
                WEND
                i = i + 1
            WEND

            MSGBOX "Sum: " & FORMAT$(sum)
        END FUNCTION
        """;

    // Mixed loop types
    public const string MixedLoops = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL j AS LONG
            LOCAL count AS LONG

            FOR i = 1 TO 3
                j = 0
                WHILE j < 3
                    count = count + 1
                    j = j + 1
                WEND

                DO
                    count = count + 1
                    IF count > 20 THEN
                        EXIT DO
                    END IF
                LOOP
            NEXT i

            MSGBOX "Count: " & FORMAT$(count)
        END FUNCTION
        """;

    // File I/O with error conditions
    public const string FileIOWithConditionals = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL fileName AS STRING
            LOCAL content AS STRING
            LOCAL i AS LONG

            fileName = "test_output.txt"

            OPEN fileName FOR OUTPUT AS #1
            FOR i = 1 TO 5
                IF i = 3 THEN
                    PRINT #1, "Special line"
                ELSE
                    PRINT #1, "Line " & FORMAT$(i)
                END IF
            NEXT i
            CLOSE #1

            OPEN fileName FOR INPUT AS #1
            WHILE NOT EOF(1)
                LINE INPUT #1, content
                MSGBOX content
            WEND
            CLOSE #1
        END FUNCTION
        """;

    // CONST in calculations
    public const string ConstCalculations = """
        #COMPILE EXE
        #DIM ALL

        CONST BASE_PRICE AS DOUBLE = 100.0
        CONST TAX_RATE AS DOUBLE = 0.08
        CONST DISCOUNT AS DOUBLE = 0.10

        FUNCTION CalculateTotal(BYVAL quantity AS LONG) AS DOUBLE
            LOCAL subtotal AS DOUBLE
            LOCAL tax AS DOUBLE
            LOCAL discount AS DOUBLE
            LOCAL total AS DOUBLE

            subtotal = BASE_PRICE * quantity
            discount = subtotal * DISCOUNT
            subtotal = subtotal - discount
            tax = subtotal * TAX_RATE
            total = subtotal + tax

            FUNCTION = total
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL finalPrice AS DOUBLE
            finalPrice = CalculateTotal(5)
            MSGBOX "Total: $" & FORMAT$(finalPrice, "0.00")
        END FUNCTION
        """;

    // Complex file operations
    public const string ComplexFileOps = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL sourceFile AS STRING
            LOCAL destFile AS STRING
            LOCAL tempFile AS STRING
            LOCAL line AS STRING

            sourceFile = "source.txt"
            destFile = "dest.txt"
            tempFile = "temp.txt"

            ' Create source file
            OPEN sourceFile FOR OUTPUT AS #1
            PRINT #1, "Line 1"
            PRINT #1, "Line 2"
            PRINT #1, "Line 3"
            CLOSE #1

            ' Copy with modification
            OPEN sourceFile FOR INPUT AS #1
            OPEN tempFile FOR OUTPUT AS #2
            WHILE NOT EOF(1)
                LINE INPUT #1, line
                PRINT #2, "Processed: " & line
            WEND
            CLOSE #1
            CLOSE #2

            ' Append to destination
            OPEN tempFile FOR INPUT AS #1
            OPEN destFile FOR APPEND AS #3
            WHILE NOT EOF(1)
                LINE INPUT #1, line
                PRINT #3, line
            WEND
            CLOSE

            MSGBOX "Files processed"
        END FUNCTION
        """;

    // WHILE with EXIT conditions
    public const string WhileWithExits = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION FindValue(BYVAL target AS LONG) AS LONG
            LOCAL i AS LONG
            i = 0

            WHILE i < 100
                IF i = target THEN
                    FUNCTION = i
                    EXIT FUNCTION
                END IF
                i = i + 1
            WEND

            FUNCTION = -1
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL result AS LONG
            result = FindValue(42)
            MSGBOX "Found at: " & FORMAT$(result)
        END FUNCTION
        """;
}