namespace Runner.Examples;

public static class EnumExamples
{
    // Simple ENUM with auto-increment
    public const string SimpleEnum = """
        #COMPILE EXE
        #DIM ALL

        ENUM Colors
            Red
            Green
            Blue
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL myColor AS LONG
            myColor = Green
            MSGBOX "Color value: " & FORMAT$(myColor)
        END FUNCTION
        """;

    // ENUM with explicit values
    public const string EnumWithValues = """
        #COMPILE EXE
        #DIM ALL

        ENUM StatusCodes
            OK = 200
            NotFound = 404
            ServerError = 500
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL status AS LONG
            status = NotFound
            MSGBOX "Status code: " & FORMAT$(status)
        END FUNCTION
        """;

    // ENUM with mixed explicit and auto-increment values
    public const string EnumMixedValues = """
        #COMPILE EXE
        #DIM ALL

        ENUM Priority
            Low = 1
            Medium
            High
            Critical = 10
            Emergency
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL level AS LONG
            level = High
            MSGBOX "Priority level: " & FORMAT$(level)
        END FUNCTION
        """;

    // ENUM with visibility modifiers
    public const string EnumWithVisibility = """
        #COMPILE EXE
        #DIM ALL

        PUBLIC ENUM DaysOfWeek
            Sunday
            Monday
            Tuesday
            Wednesday
            Thursday
            Friday
            Saturday
        END ENUM

        PRIVATE ENUM AccessLevel
            None = 0
            Read = 1
            Write = 2
            Execute = 4
            FullAccess = 7
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL day AS LONG
            LOCAL access AS LONG
            day = Wednesday
            access = FullAccess
            MSGBOX "Day: " & FORMAT$(day) & ", Access: " & FORMAT$(access)
        END FUNCTION
        """;

    // ENUM used in SELECT CASE
    public const string EnumWithSelectCase = """
        #COMPILE EXE
        #DIM ALL

        ENUM FileMode
            Read = 1
            Write = 2
            Append = 3
            ReadWrite = 4
        END ENUM

        FUNCTION GetModeString(BYVAL mode AS LONG) AS STRING
            SELECT CASE mode
                CASE Read
                    FUNCTION = "Read Only"
                CASE Write
                    FUNCTION = "Write Only"
                CASE Append
                    FUNCTION = "Append Mode"
                CASE ReadWrite
                    FUNCTION = "Read/Write"
                CASE ELSE
                    FUNCTION = "Unknown"
            END SELECT
        END FUNCTION

        FUNCTION PBMAIN() AS LONG
            LOCAL mode AS LONG
            mode = ReadWrite
            MSGBOX GetModeString(mode)
        END FUNCTION
        """;

    // ENUM with hex values
    public const string EnumWithHexValues = """
        #COMPILE EXE
        #DIM ALL

        ENUM Flags
            None = &H0
            Flag1 = &H1
            Flag2 = &H2
            Flag3 = &H4
            Flag4 = &H8
            AllFlags = &HF
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL flags AS LONG
            flags = Flag1 OR Flag3
            MSGBOX "Flags set: " & FORMAT$(flags)
        END FUNCTION
        """;

    // Multiple ENUMs in one program
    public const string MultipleEnums = """
        #COMPILE EXE
        #DIM ALL

        ENUM Color
            Red = 1
            Green = 2
            Blue = 3
        END ENUM

        ENUM Size
            Small = 10
            Medium = 20
            Large = 30
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL color AS LONG
            LOCAL size AS LONG
            color = Blue
            size = Medium
            MSGBOX "Color: " & FORMAT$(color) & ", Size: " & FORMAT$(size)
        END FUNCTION
        """;

    // ENUM starting from non-zero
    public const string EnumNonZeroStart = """
        #COMPILE EXE
        #DIM ALL

        ENUM ErrorCodes
            NoError = 100
            FileNotFound
            AccessDenied
            InvalidParameter
            OutOfMemory
        END ENUM

        FUNCTION PBMAIN() AS LONG
            LOCAL errorCode AS LONG
            errorCode = AccessDenied
            MSGBOX "Error code: " & FORMAT$(errorCode)
        END FUNCTION
        """;
}