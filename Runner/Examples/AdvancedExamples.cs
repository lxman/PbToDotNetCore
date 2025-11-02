namespace Runner.Examples;

public static class AdvancedExamples
{
    // Simple CLASS with INTERFACE and METHOD
    public const string SimpleClass = """
        #COMPILE EXE
        #DIM ALL

        CLASS MyClass
            INTERFACE MyInterface
                INHERIT IUNKNOWN
                METHOD SayHello() AS LONG
                    MSGBOX "Hello from MyClass!"
                    METHOD = 0
                END METHOD
            END INTERFACE
        END CLASS

        FUNCTION PBMAIN() AS LONG
            LOCAL obj AS MyInterface
            obj = CLASS "MyClass"
            obj.SayHello()
        END FUNCTION
        """;

    // CLASS with instance variables
    public const string ClassWithInstanceVars = """
        #COMPILE EXE
        #DIM ALL

        CLASS Counter
            INSTANCE Count AS LONG

            INTERFACE ICounter
                INHERIT IUNKNOWN

                METHOD Increment() AS LONG
                    INCR Count
                    METHOD = Count
                END METHOD

                METHOD GetValue() AS LONG
                    METHOD = Count
                END METHOD

                METHOD Reset()
                    Count = 0
                END METHOD
            END INTERFACE
        END CLASS

        FUNCTION PBMAIN() AS LONG
            LOCAL counter AS ICounter
            LOCAL value AS LONG

            counter = CLASS "Counter"
            counter.Increment()
            counter.Increment()
            counter.Increment()

            value = counter.GetValue()
            MSGBOX "Count: " & FORMAT$(value)
        END FUNCTION
        """;

    // CLASS with multiple interfaces
    public const string MultipleInterfaces = """
        #COMPILE EXE
        #DIM ALL

        CLASS Calculator
            INSTANCE LastResult AS DOUBLE

            INTERFACE IMath
                INHERIT IUNKNOWN

                METHOD Add(BYVAL a AS DOUBLE, BYVAL b AS DOUBLE) AS DOUBLE
                    LastResult = a + b
                    METHOD = LastResult
                END METHOD

                METHOD Multiply(BYVAL a AS DOUBLE, BYVAL b AS DOUBLE) AS DOUBLE
                    LastResult = a * b
                    METHOD = LastResult
                END METHOD
            END INTERFACE

            INTERFACE IHistory
                INHERIT IUNKNOWN

                METHOD GetLastResult() AS DOUBLE
                    METHOD = LastResult
                END METHOD
            END INTERFACE
        END CLASS

        FUNCTION PBMAIN() AS LONG
            LOCAL math AS IMath
            LOCAL history AS IHistory
            LOCAL result AS DOUBLE

            math = CLASS "Calculator"
            result = math.Add(10, 5)

            history = math
            result = history.GetLastResult()

            MSGBOX "Last result: " & FORMAT$(result)
        END FUNCTION
        """;

    // CLASS with UNION type
    public const string ClassWithUnion = """
        #COMPILE EXE
        #DIM ALL

        TYPE DataUnion
            UNION
                LongValue AS LONG
                DWordValue AS DWORD
                ByteArray(4) AS BYTE
            END UNION
        END TYPE

        FUNCTION PBMAIN() AS LONG
            LOCAL data AS DataUnion

            data.LongValue = -1
            MSGBOX "LONG: " & FORMAT$(data.LongValue) & $CRLF & _
                   "DWORD: " & FORMAT$(data.DWordValue)
        END FUNCTION
        """;

    // CLASS with pointer operations
    public const string PointerExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL value AS LONG
            LOCAL pValue AS LONG PTR

            value = 42
            pValue = VARPTR(value)

            MSGBOX "Value: " & FORMAT$(value) & $CRLF & _
                   "Via pointer: " & FORMAT$(@pValue)

            @pValue = 100
            MSGBOX "Updated value: " & FORMAT$(value)
        END FUNCTION
        """;

    // Inline assembly example
    public const string InlineAssembly = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL result AS LONG

            ! MOV EAX, 100
            ! ADD EAX, 50
            ! MOV result, EAX

            MSGBOX "Assembly result: " & FORMAT$(result)
        END FUNCTION
        """;

    // MACRO example
    public const string MacroExample = """
        #COMPILE EXE
        #DIM ALL

        MACRO Square(x)
            ((x) * (x))
        END MACRO

        FUNCTION PBMAIN() AS LONG
            LOCAL num AS LONG
            LOCAL result AS LONG

            num = 5
            result = Square(num)

            MSGBOX "Square of " & FORMAT$(num) & " is " & FORMAT$(result)
        END FUNCTION
        """;

    // Thread example - Note: THREAD CREATE with function arguments not supported
    public const string ThreadExample = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION WorkerThread(BYVAL param AS DWORD) AS LONG
            LOCAL i AS LONG
            FOR i = 1 TO 5
                SLEEP 500
            NEXT i
            FUNCTION = 0
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL hThread AS DWORD
            THREAD CREATE WorkerThread TO hThread
            MSGBOX "Thread started"
            THREAD WAIT hThread
            MSGBOX "Thread completed"
            THREAD CLOSE hThread
        END FUNCTION
        """;

    // Bit operations: SHIFT and ROTATE
    public const string BitOperations = """
        #COMPILE EXE
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL value AS DWORD
            LOCAL shifted AS DWORD
            LOCAL rotated AS DWORD

            value = %00001111

            shifted = value
            SHIFT LEFT shifted, 2

            rotated = value
            ROTATE LEFT rotated, 2

            MSGBOX "Original: " & BIN$(value) & $CRLF & _
                   "Shifted left 2: " & BIN$(shifted) & $CRLF & _
                   "Rotated left 2: " & BIN$(rotated)
        END FUNCTION
        """;

    // Event handling example
    public const string SourceCode1 = """
        #COMPILE EXE
        #DIM ALL

        CLASS EvClass AS EVENT
            INTERFACE IStatus AS EVENT
                INHERIT IUNKNOWN
                METHOD Done
                    ? "Done!"
                END METHOD
            END INTERFACE
        END CLASS

        CLASS MyClass
            INTERFACE IMath
                INHERIT IUNKNOWN
                METHOD DoMath
                    ? "Calculating..."
                    RAISEEVENT IStatus.Done()
                END METHOD
            END INTERFACE
            EVENT SOURCE IStatus
        END CLASS

        FUNCTION PBMAIN()
            LOCAL oMath AS IMath
            LOCAL oStatus AS IStatus

            oMath = CLASS "MyClass"
            oStatus = CLASS "EvClass"

            EVENTS FROM oMath CALL oStatus

            oMath.DoMath

            EVENTS END oStatus
        END FUNCTION
        """;
}