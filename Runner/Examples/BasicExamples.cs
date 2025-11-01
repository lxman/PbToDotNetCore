namespace Runner.Examples;

public static class BasicExamples
{
    // Simple Hello World
    public const string HelloWorld = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            MSGBOX "Hello World"
        END FUNCTION
        """;

    // Function with parameters and local variables
    public const string FunctionWithParams = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION Add(BYVAL x AS LONG, BYVAL y AS LONG) AS LONG
            LOCAL result AS LONG
            result = x + y
            Add = result
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL answer AS LONG
            answer = Add(5, 10)
            MSGBOX "5 + 10 = " & FORMAT$(answer)
        END FUNCTION
        """;

    // Variables and data types
    public const string VariablesExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL name AS STRING
            LOCAL age AS LONG
            LOCAL price AS DOUBLE
            LOCAL flag AS LONG

            name = "PowerBASIC"
            age = 25
            price = 99.95
            flag = 1

            MSGBOX "Name: " & name & $CRLF & _
                   "Age: " & FORMAT$(age) & $CRLF & _
                   "Price: $" & FORMAT$(price, "#.00")
        END FUNCTION
        """;

    // SUB and CALL statements
    public const string SubExample = """
        #COMPILE EXE
        #DIM ALL

        SUB PrintMessage(msg AS STRING)
            MSGBOX msg
        END SUB

        FUNCTION PBMAIN() AS LONG
            CALL PrintMessage("Hello from SUB")
        END FUNCTION
        """;

    // Loops: FOR/NEXT
    public const string ForLoopExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL i AS LONG
            LOCAL result AS STRING

            FOR i = 1 TO 10
                result = result & FORMAT$(i) & " "
            NEXT i

            MSGBOX "Numbers 1-10: " & result
        END FUNCTION
        """;

    // IF/THEN/ELSE
    public const string IfThenExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL number AS LONG
            LOCAL msg AS STRING

            number = 42

            IF number > 50 THEN
                msg = "Greater than 50"
            ELSEIF number > 25 THEN
                msg = "Between 25 and 50"
            ELSE
                msg = "25 or less"
            END IF

            MSGBOX msg
        END FUNCTION
        """;

    // SELECT CASE
    public const string SelectCaseExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL dayNum AS LONG
            LOCAL dayName AS STRING

            dayNum = 3

            SELECT CASE dayNum
                CASE 1
                    dayName = "Monday"
                CASE 2
                    dayName = "Tuesday"
                CASE 3
                    dayName = "Wednesday"
                CASE 4
                    dayName = "Thursday"
                CASE 5
                    dayName = "Friday"
                CASE ELSE
                    dayName = "Weekend"
            END SELECT

            MSGBOX "Day " & FORMAT$(dayNum) & " is " & dayName
        END FUNCTION
        """;

    // Arrays
    public const string ArrayExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL numbers(10) AS LONG
            LOCAL i AS LONG
            LOCAL total AS LONG

            FOR i = 1 TO 10
                numbers(i) = i * 2
                total = total + numbers(i)
            NEXT i

            MSGBOX "Sum of even numbers 2-20: " & FORMAT$(total)
        END FUNCTION
        """;

    // DLL export function
    public const string SourceCode1 = """
        #COMPILE DLL
        #DIM ALL

        FUNCTION RandomAdd ALIAS "RandomAdd" (BYVAL x AS SINGLE) EXPORT AS LONG
            RandomAdd = x + RND
        END FUNCTION
        """;
}