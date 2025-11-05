namespace Runner.Examples;

public static class RedimExamples
{
    // Simple REDIM example
    public const string SimpleRedim = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM myArray(5) AS LONG
            LOCAL i AS LONG

            ' Fill initial array
            FOR i = 0 TO 5
                myArray(i) = i * 10
            NEXT i

            ' Resize array (loses data)
            REDIM myArray(10)

            ' Fill new array
            FOR i = 0 TO 10
                myArray(i) = i * 5
            NEXT i

            MSGBOX "Array resized and filled"
        END FUNCTION
        """;

    // REDIM PRESERVE example
    public const string RedimPreserve = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM numbers(3) AS LONG
            LOCAL i AS LONG

            ' Fill initial array
            FOR i = 0 TO 3
                numbers(i) = i + 1
            NEXT i

            ' Resize array keeping existing data
            REDIM PRESERVE numbers(6)

            ' Add new values
            FOR i = 4 TO 6
                numbers(i) = i + 1
            NEXT i

            ' Display all values
            FOR i = 0 TO 6
                MSGBOX "numbers(" & FORMAT$(i) & ") = " & FORMAT$(numbers(i))
            NEXT i
        END FUNCTION
        """;

    // REDIM with different types
    public const string RedimDifferentTypes = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM names(2) AS STRING
            DIM values(2) AS DOUBLE

            names(0) = "Alpha"
            names(1) = "Beta"
            names(2) = "Gamma"

            values(0) = 1.5
            values(1) = 2.5
            values(2) = 3.5

            ' Resize both arrays
            REDIM PRESERVE names(4)
            REDIM PRESERVE values(4)

            names(3) = "Delta"
            names(4) = "Epsilon"

            values(3) = 4.5
            values(4) = 5.5

            MSGBOX "Arrays resized"
        END FUNCTION
        """;

    // REDIM in loop
    public const string RedimInLoop = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM data() AS LONG
            LOCAL size AS LONG
            LOCAL i AS LONG

            FOR size = 1 TO 3
                REDIM data(size * 2)

                FOR i = 0 TO size * 2
                    data(i) = i * size
                NEXT i

                MSGBOX "Array size: " & FORMAT$(size * 2 + 1)
            NEXT size
        END FUNCTION
        """;

    // Dynamic array growth pattern
    public const string DynamicGrowth = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM items() AS LONG
            LOCAL count AS LONG
            LOCAL value AS LONG
            LOCAL i AS LONG

            count = 0
            REDIM items(0)

            ' Simulate adding items dynamically
            FOR i = 1 TO 5
                value = i * 100

                ' Grow array
                REDIM PRESERVE items(count)
                items(count) = value
                count = count + 1

                MSGBOX "Added: " & FORMAT$(value)
            NEXT i

            MSGBOX "Total items: " & FORMAT$(count)
        END FUNCTION
        """;

    // Multi-dimensional REDIM
    public const string MultiDimensionalRedim = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM matrix(2, 2) AS LONG
            LOCAL i AS LONG
            LOCAL j AS LONG

            ' Fill initial matrix
            FOR i = 0 TO 2
                FOR j = 0 TO 2
                    matrix(i, j) = i * 10 + j
                NEXT j
            NEXT i

            ' Resize matrix (loses data in PowerBASIC)
            REDIM matrix(3, 3)

            ' Fill new matrix
            FOR i = 0 TO 3
                FOR j = 0 TO 3
                    matrix(i, j) = i * 20 + j
                NEXT j
            NEXT i

            MSGBOX "Matrix resized to 4x4"
        END FUNCTION
        """;

    // REDIM with AS type clause
    public const string RedimWithType = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM buffer() AS BYTE

            ' Initial size
            REDIM buffer(255) AS BYTE

            ' Fill with data
            LOCAL i AS LONG
            FOR i = 0 TO 255
                buffer(i) = i MOD 256
            NEXT i

            ' Resize to larger buffer
            REDIM buffer(511) AS BYTE

            MSGBOX "Buffer resized"
        END FUNCTION
        """;

    // REDIM multiple arrays in one statement
    public const string RedimMultiple = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            DIM arr1() AS LONG
            DIM arr2() AS LONG
            DIM arr3() AS LONG

            ' Resize multiple arrays at once
            REDIM arr1(5), arr2(10), arr3(15)

            ' Use the arrays
            arr1(0) = 100
            arr2(0) = 200
            arr3(0) = 300

            MSGBOX "Multiple arrays resized"
        END FUNCTION
        """;
}