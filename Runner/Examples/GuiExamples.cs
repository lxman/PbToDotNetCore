namespace Runner.Examples;

public static class GuiExamples
{
    // Simple dialog with button
    public const string SimpleDialog = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        FUNCTION PBMAIN() AS LONG
            LOCAL hDlg AS DWORD

            DIALOG NEW 0, "PowerBASIC", 300, 300, 100, 75, %WS_SYSMENU, 0 TO hDlg
            CONTROL ADD BUTTON, hDlg, 2, "Cancel", 25, 15, 40, 20

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            IF CBMSG = %WM_COMMAND THEN
                DIALOG END CBHNDL, 0
            END IF
        END FUNCTION
        """;

    // Dialog with multiple controls
    public const string MultipleControls = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        GLOBAL hDlg AS DWORD

        FUNCTION PBMAIN() AS LONG
            DIALOG NEW 0, "Form Demo", 100, 100, 200, 150, %WS_OVERLAPPEDWINDOW, 0 TO hDlg

            CONTROL ADD LABEL, hDlg, 101, "Name:", 10, 10, 50, 15
            CONTROL ADD TEXTBOX, hDlg, 102, "", 70, 10, 100, 15

            CONTROL ADD LABEL, hDlg, 103, "Age:", 10, 35, 50, 15
            CONTROL ADD TEXTBOX, hDlg, 104, "", 70, 35, 50, 15

            CONTROL ADD CHECKBOX, hDlg, 105, "Subscribe to newsletter", 10, 60, 150, 15

            CONTROL ADD BUTTON, hDlg, 106, "Submit", 10, 90, 60, 25
            CONTROL ADD BUTTON, hDlg, 107, "Cancel", 80, 90, 60, 25

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            LOCAL name AS STRING
            LOCAL age AS STRING

            SELECT CASE CBMSG
                CASE %WM_COMMAND
                    SELECT CASE CBCTL
                        CASE 106
                            CONTROL GET TEXT hDlg, 102 TO name
                            CONTROL GET TEXT hDlg, 104 TO age
                            MSGBOX "Name: " & name & $CRLF & "Age: " & age
                        CASE 107
                            DIALOG END hDlg, 0
                    END SELECT
            END CASE
        END FUNCTION
        """;

    // Dialog with listbox
    public const string ListBoxExample = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        GLOBAL hDlg AS DWORD

        FUNCTION PBMAIN() AS LONG
            DIALOG NEW 0, "Listbox Demo", 100, 100, 200, 200, %WS_OVERLAPPEDWINDOW, 0 TO hDlg

            CONTROL ADD LISTBOX, hDlg, 101, "", 10, 10, 180, 100
            CONTROL ADD BUTTON, hDlg, 102, "Add Item", 10, 120, 60, 25
            CONTROL ADD BUTTON, hDlg, 103, "Remove", 80, 120, 60, 25
            CONTROL ADD BUTTON, hDlg, 104, "Close", 150, 120, 40, 25

            LISTBOX ADD hDlg, 101, "Item 1"
            LISTBOX ADD hDlg, 101, "Item 2"
            LISTBOX ADD hDlg, 101, "Item 3"

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            LOCAL count AS LONG

            SELECT CASE CBMSG
                CASE %WM_COMMAND
                    SELECT CASE CBCTL
                        CASE 102
                            LISTBOX GET COUNT hDlg, 101 TO count
                            LISTBOX ADD hDlg, 101, "Item " & FORMAT$(count + 1)
                        CASE 103
                            LISTBOX GET SELECT hDlg, 101 TO count
                            IF count >= 0 THEN
                                LISTBOX DELETE hDlg, 101, count
                            END IF
                        CASE 104
                            DIALOG END hDlg, 0
                    END SELECT
            END CASE
        END FUNCTION
        """;

    // Dialog with combobox
    public const string ComboBoxExample = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        GLOBAL hDlg AS DWORD

        FUNCTION PBMAIN() AS LONG
            DIALOG NEW 0, "ComboBox Demo", 100, 100, 250, 120, %WS_OVERLAPPEDWINDOW, 0 TO hDlg

            CONTROL ADD LABEL, hDlg, 101, "Select Color:", 10, 10, 80, 15
            CONTROL ADD COMBOBOX, hDlg, 102, "", 100, 10, 120, 100

            COMBOBOX ADD hDlg, 102, "Red"
            COMBOBOX ADD hDlg, 102, "Green"
            COMBOBOX ADD hDlg, 102, "Blue"
            COMBOBOX ADD hDlg, 102, "Yellow"

            CONTROL ADD BUTTON, hDlg, 103, "Show Selection", 10, 40, 100, 25
            CONTROL ADD BUTTON, hDlg, 104, "Close", 120, 40, 60, 25

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            LOCAL selection AS STRING
            LOCAL index AS LONG

            SELECT CASE CBMSG
                CASE %WM_COMMAND
                    SELECT CASE CBCTL
                        CASE 103
                            COMBOBOX GET SELECT hDlg, 102 TO index
                            IF index >= 0 THEN
                                COMBOBOX GET TEXT hDlg, 102, index TO selection
                                MSGBOX "You selected: " & selection
                            END IF
                        CASE 104
                            DIALOG END hDlg, 0
                    END SELECT
            END CASE
        END FUNCTION
        """;

    // Dialog with menu
    public const string MenuExample = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        GLOBAL hDlg AS DWORD

        FUNCTION PBMAIN() AS LONG
            DIALOG NEW 0, "Menu Demo", 100, 100, 300, 200, %WS_OVERLAPPEDWINDOW, 0 TO hDlg

            MENU NEW BAR TO hDlg
            MENU NEW POPUP TO hDlg
            MENU ADD STRING, "&New", 101, %MF_ENABLED
            MENU ADD STRING, "&Open", 102, %MF_ENABLED
            MENU ADD STRING, "-", 0, %MF_SEPARATOR
            MENU ADD STRING, "E&xit", 199, %MF_ENABLED
            MENU END POPUP
            MENU ATTACH POPUP hDlg, "&File"

            MENU NEW POPUP TO hDlg
            MENU ADD STRING, "&About", 201, %MF_ENABLED
            MENU END POPUP
            MENU ATTACH POPUP hDlg, "&Help"

            CONTROL ADD LABEL, hDlg, 301, "Select a menu item", 10, 10, 280, 20

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            SELECT CASE CBMSG
                CASE %WM_COMMAND
                    SELECT CASE CBCTL
                        CASE 101
                            MSGBOX "New file"
                        CASE 102
                            MSGBOX "Open file"
                        CASE 199
                            DIALOG END hDlg, 0
                        CASE 201
                            MSGBOX "PowerBASIC Menu Demo" & $CRLF & "Version 1.0"
                    END SELECT
            END CASE
        END FUNCTION
        """;

    // Graphic window example
    public const string GraphicWindow = """
        #COMPILE EXE
        #INCLUDE "Win32API.inc"
        #DIM ALL

        GLOBAL hDlg AS DWORD

        FUNCTION PBMAIN() AS LONG
            DIALOG NEW 0, "Graphics Demo", 100, 100, 400, 300, %WS_OVERLAPPEDWINDOW, 0 TO hDlg

            CONTROL ADD GRAPHIC, hDlg, 101, "", 10, 10, 370, 240

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            LOCAL i AS LONG

            SELECT CASE CBMSG
                CASE %WM_INITDIALOG
                    GRAPHIC ATTACH hDlg, 101
                    GRAPHIC COLOR %RGB_BLUE, %RGB_WHITE
                    GRAPHIC CLEAR

                    FOR i = 10 TO 100 STEP 10
                        GRAPHIC ELLIPSE (i, i)-(200-i, 200-i)
                    NEXT i

                    GRAPHIC COLOR %RGB_RED, 0
                    GRAPHIC LINE (10, 10)-(190, 190)

                CASE %WM_COMMAND
                    IF CBCTL = %IDCANCEL THEN
                        DIALOG END hDlg, 0
                    END IF
            END CASE
        END FUNCTION
        """;

    // Original complex example
    public const string SourceCode1 = """
        #COMPILE EXE
        #REGISTER NONE
        #DIM ALL
        #INCLUDE "win32api.inc"

        FUNCTION PBMAIN() AS LONG
            LOCAL hDlg AS DWORD

            DIALOG NEW %HWND_DESKTOP, "My PowerBASIC Window", 100, 100, 300, 200, %WS_OVERLAPPEDWINDOW TO hDlg

            MENU NEW BAR TO hDlg
            MENU NEW POPUP TO hDlg
            MENU ADD STRING, "E&xit", 101, %MF_ENABLED
            MENU END POPUP
            MENU ATTACH POPUP hDlg, "&File"

            MENU NEW POPUP TO hDlg
            MENU ADD STRING, "&About", 201, %MF_ENABLED
            MENU END POPUP
            MENU ATTACH POPUP hDlg, "&Help"

            CONTROL ADD BUTTON, hDlg, 1001, "Click Me!", 10, 10, 80, 25
            CONTROL ADD LABEL, hDlg, 1002, "Welcome!", 10, 50, 280, 20

            DIALOG SHOW MODAL hDlg CALL DlgProc TO FUNCTION
        END FUNCTION

        CALLBACK FUNCTION DlgProc() AS LONG
            SELECT CASE CBMSG
                CASE %WM_COMMAND
                    SELECT CASE CBCTL
                        CASE 1001
                            MSGBOX "Button clicked!"
                        CASE 101
                            DIALOG END CBHNDL, 0
                        CASE 201
                            MSGBOX "PowerBASIC Example Application"
                    END SELECT
            END CASE
        END FUNCTION
        """;
}