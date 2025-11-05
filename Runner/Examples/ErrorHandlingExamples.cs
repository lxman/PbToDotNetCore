namespace Runner.Examples;

public static class ErrorHandlingExamples
{
    // Simple ON ERROR GOTO example
    public const string OnErrorGoto = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            ON ERROR GOTO ErrorHandler

            LOCAL x AS LONG
            LOCAL y AS LONG
            x = 10
            y = 0

            ' This will cause a division by zero error
            x = x / y

            MSGBOX "This should not display"
            EXIT FUNCTION

        ErrorHandler:
            MSGBOX "An error occurred!"
            RESUME NEXT
        END FUNCTION
        """;

    // ON ERROR RESUME NEXT example
    public const string OnErrorResumeNext = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            ON ERROR RESUME NEXT

            LOCAL x AS LONG
            LOCAL y AS LONG
            x = 10
            y = 0

            ' This error will be ignored
            x = x / y

            MSGBOX "Execution continues despite error"
        END FUNCTION
        """;

    // ERROR statement to raise custom error
    public const string RaiseError = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            ON ERROR GOTO ErrorHandler

            LOCAL errorCode AS LONG
            errorCode = 100

            ' Raise a custom error
            ERROR errorCode

            MSGBOX "This should not display"
            EXIT FUNCTION

        ErrorHandler:
            MSGBOX "Custom error raised"
        END FUNCTION
        """;

    // Multiple error handlers
    public const string MultipleErrorHandlers = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION ProcessData(BYVAL value AS LONG) AS LONG
            ON ERROR GOTO ProcessError

            IF value = 0 THEN
                ERROR 5  ' Invalid argument error
            END IF

            FUNCTION = 100 / value
            EXIT FUNCTION

        ProcessError:
            FUNCTION = -1
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL result AS LONG

            result = ProcessData(0)
            MSGBOX "Result: " & FORMAT$(result)
        END FUNCTION
        """;

    // RESUME statement variations
    public const string ResumeVariations = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL counter AS LONG
            LOCAL value AS LONG

            ON ERROR GOTO ErrorHandler

        TryAgain:
            counter = counter + 1

            IF counter = 1 THEN
                ERROR 100  ' First error
            ELSEIF counter = 2 THEN
                ERROR 200  ' Second error
            END IF

            MSGBOX "Success on attempt " & FORMAT$(counter)
            EXIT FUNCTION

        ErrorHandler:
            IF counter < 3 THEN
                RESUME TryAgain  ' Resume at label
            ELSE
                MSGBOX "Too many errors"
            END IF
        END FUNCTION
        """;

    // Error in loop with RESUME NEXT
    public const string ErrorInLoop = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL result AS LONG

            ON ERROR RESUME NEXT

            FOR i = -2 TO 2
                result = 10 / i  ' Error when i = 0
                MSGBOX "10 / " & FORMAT$(i) & " = " & FORMAT$(result)
            NEXT i

            MSGBOX "Loop completed"
        END FUNCTION
        """;

    // Nested error handling
    public const string NestedErrorHandling = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION InnerFunction() AS LONG
            ON ERROR GOTO InnerError

            ERROR 50
            FUNCTION = 1
            EXIT FUNCTION

        InnerError:
            FUNCTION = -1
        END FUNCTION

        FUNCTION OuterFunction() AS LONG
            ON ERROR GOTO OuterError

            LOCAL result AS LONG
            result = InnerFunction()

            IF result = -1 THEN
                ERROR 100
            END IF

            FUNCTION = result
            EXIT FUNCTION

        OuterError:
            FUNCTION = -2
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL finalResult AS LONG
            finalResult = OuterFunction()
            MSGBOX "Final result: " & FORMAT$(finalResult)
        END FUNCTION
        """;
}