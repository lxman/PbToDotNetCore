namespace Runner.Examples;

public static class ExitExamples
{
    // Simple EXIT FOR example
    public const string ExitFor = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG

            FOR i = 1 TO 100
                IF i = 42 THEN
                    MSGBOX "Found 42!"
                    EXIT FOR
                END IF
            NEXT i

            MSGBOX "Done"
        END FUNCTION
        """;

    // EXIT FUNCTION example
    public const string ExitFunction = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION ValidateAge(BYVAL age AS LONG) AS LONG
            IF age < 0 THEN
                EXIT FUNCTION
            END IF

            FUNCTION = %TRUE
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL result AS LONG
            result = ValidateAge(-5)
            MSGBOX FORMAT$(result)
        END FUNCTION
        """;

    // Simple FUNCTION assignment test (without EXIT)
    public const string FunctionAssignment = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION GetValue() AS LONG
            FUNCTION = 42
        END FUNCTION
        """;

    // EXIT SUB example
    public const string ExitSub = """
        #COMPILE EXE
        #DIM ALL

        SUB ProcessValue(BYVAL value AS LONG)
            IF value = 0 THEN
                EXIT SUB
            END IF
            MSGBOX "Processing: " & FORMAT$(value)
        END SUB

        FUNCTION PBMAIN() AS LONG
            ProcessValue(0)
            ProcessValue(10)
        END FUNCTION
        """;

    // EXIT DO example
    public const string ExitDo = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL counter AS LONG

            DO
                INCR counter
                IF counter > 10 THEN
                    EXIT DO
                END IF
            LOOP

            MSGBOX "Counter: " & FORMAT$(counter)
        END FUNCTION
        """;
}
