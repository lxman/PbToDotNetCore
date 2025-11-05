namespace Runner.Examples;

public static class ConstAndWhileExamples
{
    // CONST statement examples
    public const string SimpleConst = """
        #COMPILE EXE
        #DIM ALL

        CONST PI = 3.14159
        CONST MAX_COUNT = 100
        CONST MESSAGE = "Hello, World!"

        FUNCTION PBMAIN() AS LONG
            MSGBOX MESSAGE
            MSGBOX FORMAT$(PI)
            MSGBOX FORMAT$(MAX_COUNT)
        END FUNCTION
        """;

    public const string ConstWithTypes = """
        #COMPILE EXE
        #DIM ALL

        CONST PI AS DOUBLE = 3.14159265359
        CONST MAX_SIZE AS LONG = 1024
        CONST APP_NAME AS STRING = "My Application"
        CONST IS_DEBUG AS LONG = 1

        FUNCTION PBMAIN() AS LONG
            MSGBOX APP_NAME & " - Max Size: " & FORMAT$(MAX_SIZE)
        END FUNCTION
        """;

    public const string ConstWithVisibility = """
        #COMPILE EXE
        #DIM ALL

        PUBLIC CONST VERSION = "1.0.0"
        PRIVATE CONST SECRET_KEY = "XYZ123"
        GLOBAL CONST APP_ID = 42

        FUNCTION PBMAIN() AS LONG
            MSGBOX "Version: " & VERSION
        END FUNCTION
        """;

    // WHILE...WEND loop examples
    public const string SimpleWhileWend = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL counter AS LONG
            counter = 0

            WHILE counter < 5
                MSGBOX "Counter: " & FORMAT$(counter)
                INCR counter
            WEND

            MSGBOX "Done!"
        END FUNCTION
        """;

    public const string WhileWendWithCondition = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL x AS LONG
            LOCAL sum AS LONG
            x = 1
            sum = 0

            WHILE x <= 10
                sum = sum + x
                x = x + 1
            WEND

            MSGBOX "Sum of 1 to 10: " & FORMAT$(sum)
        END FUNCTION
        """;

    public const string WhileWendWithBreak = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0

            WHILE i < 100
                IF i = 42 THEN
                    EXIT FUNCTION
                END IF
                INCR i
            WEND

            MSGBOX "This should not appear"
        END FUNCTION
        """;

    // Multiple CONST declarations in one line
    public const string ConstMultipleDeclarations = """
        #COMPILE EXE
        #DIM ALL

        CONST A = 1, B = 2, C = 3
        CONST X AS STRING = "Test"

        FUNCTION PBMAIN() AS LONG
            MSGBOX FORMAT$(A + B + C) & " " & X
        END FUNCTION
        """;

    // Nested WHILE loops
    public const string NestedWhileLoops = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL j AS LONG
            LOCAL count AS LONG

            i = 0
            WHILE i < 3
                j = 0
                WHILE j < 2
                    count = count + 1
                    j = j + 1
                WEND
                i = i + 1
            WEND

            MSGBOX "Count: " & FORMAT$(count)
        END FUNCTION
        """;

    // Combined example
    public const string ConstAndWhileCombined = """
        #COMPILE EXE
        #DIM ALL

        CONST MAX_ITERATIONS AS LONG = 10
        CONST INCREMENT_VALUE AS LONG = 2

        FUNCTION PBMAIN() AS LONG
            LOCAL counter AS LONG
            LOCAL result AS LONG
            counter = 0
            result = 0

            WHILE counter < MAX_ITERATIONS
                result = result + INCREMENT_VALUE
                INCR counter
            WEND

            MSGBOX "Result: " & FORMAT$(result)
        END FUNCTION
        """;
}