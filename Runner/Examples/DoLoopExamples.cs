namespace Runner.Examples;

public static class DoLoopExamples
{
    // Simple DO...LOOP (infinite loop)
    public const string SimpleDoLoop = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0
            DO
                i = i + 1
            LOOP
        END FUNCTION
        """;

    // DO WHILE...LOOP
    public const string DoWhileLoop = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0
            DO WHILE i < 5
                i = i + 1
            LOOP
            MSGBOX FORMAT$(i)
        END FUNCTION
        """;

    // DO UNTIL...LOOP
    public const string DoUntilLoop = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0
            DO UNTIL i >= 5
                i = i + 1
            LOOP
            MSGBOX FORMAT$(i)
        END FUNCTION
        """;

    // DO...LOOP WHILE (condition at end)
    public const string DoLoopWhile = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0
            DO
                i = i + 1
            LOOP WHILE i < 5
            MSGBOX FORMAT$(i)
        END FUNCTION
        """;

    // DO...LOOP UNTIL (condition at end)
    public const string DoLoopUntil = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            i = 0
            DO
                i = i + 1
            LOOP UNTIL i >= 5
            MSGBOX FORMAT$(i)
        END FUNCTION
        """;
}