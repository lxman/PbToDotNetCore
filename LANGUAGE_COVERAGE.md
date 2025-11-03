# PowerBASIC Language Coverage Catalog

This document catalogs all PowerBASIC statement types and their conversion status.

## Summary Statistics

**Total Statement Types in Grammar:** 77
**Implemented:** 22
**Not Implemented:** 55
**Coverage:** ~29%

---

## ✅ Implemented Statements

### Core Control Flow
- ✅ **forNextStmt** - FOR...NEXT loops (with inline variable declaration)
- ✅ **ifBlockStmt** - IF...THEN...END IF blocks
- ✅ **ifElseIfBlockStmt** - ELSE IF blocks
- ✅ **ifElseBlockStmt** - ELSE blocks
- ✅ **ifConditionStmt** - IF condition evaluation
- ✅ **selectCaseStmt** - SELECT CASE statements → switch

### Variable & Data
- ✅ **letStmt** - LET assignments (implicit assignment)
- ✅ **variableStmt** - LOCAL variable declarations
- ✅ **constStmt** - CONST declarations (partial - needs verification)
- ✅ **incrStmt** - INCR statement → ++
- ✅ **decrStmt** - DECR statement → --

### Functions & Procedures
- ✅ **functionStmt** - FUNCTION declarations
- ✅ **subStmt** - SUB declarations
- ✅ **methodStmt** - CLASS METHOD declarations
- ✅ **explicitCallStmt** - Explicit function calls
- ✅ **implicitCallStmt_InStmt** - Implicit function calls

### Object-Oriented
- ✅ **classStmt** - CLASS declarations
- ✅ **interfaceStmt** - INTERFACE declarations
- ✅ **typeStmt** - TYPE (struct) declarations
- ✅ **unionStmt** - UNION declarations (within TYPE)

### Bitwise Operations
- ✅ **shiftStmt** - SHIFT LEFT/RIGHT → <<=, >>=
- ✅ **rotateStmt** - ROTATE LEFT/RIGHT → bitwise rotation

### I/O
- ✅ **printStmt** - PRINT statement → Console.WriteLine

---

## ⚠️ Partially Supported

### Advanced Features (Comment Generation Only)
- ⚠️ **macroStmt** - MACRO definitions → File rejection with helpful message
- ⚠️ **threadStmt** - THREAD CREATE/CLOSE → Grouped comment blocks
- ⚠️ **asmStmt** - Inline assembly (!) → Grouped comment blocks

---

## ❌ Not Implemented Statements

### Loop Control
- ❌ **doLoopStmt** - DO...LOOP [WHILE|UNTIL]
- ❌ **whileWendStmt** - WHILE...WEND loops
- ❌ **forEachStmt** - FOR EACH loops
- ❌ **exitStmt** - EXIT FOR, EXIT FUNCTION, EXIT SUB
- ❌ **returnStmt** - RETURN statement

### Error Handling
- ❌ **onErrorStmt** - ON ERROR GOTO/RESUME
- ❌ **errorStmt** - ERROR statement (raise error)
- ❌ **resumeStmt** - RESUME statement
- ❌ **raiseEventStmt** - RAISEEVENT statement

### File I/O
- ❌ **openStmt** - OPEN file
- ❌ **closeStmt** - CLOSE file
- ❌ **inputStmt** - INPUT from file
- ❌ **lineInputStmt** - LINE INPUT from file
- ❌ **getStmt** - GET from file
- ❌ **putStmt** - PUT to file
- ❌ **writeStmt** - WRITE to file
- ❌ **seekStmt** - SEEK file position
- ❌ **lockStmt** - LOCK file regions
- ❌ **unlockStmt** - UNLOCK file regions
- ❌ **resetStmt** - RESET (close all files)
- ❌ **widthStmt** - WIDTH (file width)

### File System Operations
- ❌ **filecopyStmt** - FILECOPY
- ❌ **killStmt** - KILL (delete file)
- ❌ **nameStmt** - NAME (rename file)
- ❌ **chDirStmt** - CHDIR
- ❌ **mkdirStmt** - MKDIR
- ❌ **rmdirStmt** - RMDIR
- ❌ **chDriveStmt** - CHDRIVE

### GUI/Windows-Specific
- ❌ **loadStmt** - LOAD (load form)
- ❌ **unloadStmt** - UNLOAD (unload form)
- ❌ **sendkeysStmt** - SENDKEYS
- ❌ **appActivateStmt** - APP ACTIVATE
- ❌ **beepStmt** - BEEP
- ❌ **savepictureStmt** - SAVEPICTURE

### Registry/Settings
- ❌ **saveSettingStmt** - SAVESETTING
- ❌ **deleteSettingStmt** - DELETESETTING

### String Operations
- ❌ **lsetStmt** - LSET (left-justify string)
- ❌ **rsetStmt** - RSET (right-justify string)
- ❌ **midStmt** - MID$ assignment

### Array & Memory
- ❌ **redimStmt** - REDIM (resize array)
- ❌ **redimSubStmt** - REDIM substatement
- ❌ **eraseStmt** - ERASE (clear array)

### Flow Control
- ❌ **goToStmt** - GOTO
- ❌ **goSubStmt** - GOSUB
- ❌ **onGoToStmt** - ON...GOTO
- ❌ **onGoSubStmt** - ON...GOSUB
- ❌ **endStmt** - END (terminate program)
- ❌ **stopStmt** - STOP (break to debugger)

### Type & Defaults
- ❌ **deftypeStmt** - DEFINT, DEFLNG, etc.
- ❌ **declareStmt** - DECLARE (external function)
- ❌ **implementsStmt** - IMPLEMENTS

### Object/Component
- ❌ **setStmt** - SET object reference
- ❌ **withStmt** - WITH...END WITH
- ❌ **typeOfStmt** - TYPEOF
- ❌ **eventStmt** - EVENT declaration

### Property Procedures
- ❌ **propertyGetStmt** - PROPERTY GET
- ❌ **propertySetStmt** - PROPERTY SET
- ❌ **propertyLetStmt** - PROPERTY LET

### Date/Time
- ❌ **dateStmt** - DATE$ assignment
- ❌ **timeStmt** - TIME$ assignment

### Miscellaneous
- ❌ **randomizeStmt** - RANDOMIZE
- ❌ **setattrStmt** - SETATTR (file attributes)
- ❌ **enumerationStmt** - ENUM declarations
- ❌ **attributeStmt** - ATTRIBUTE statements

### Preprocessor
- ❌ **macroIfThenElseStmt** - #IF...#ENDIF
- ❌ **macroIfBlockStmt** - #IF blocks
- ❌ **macroElseIfBlockStmt** - #ELSEIF blocks
- ❌ **macroElseBlockStmt** - #ELSE blocks

---

## Priority Recommendations

### High Priority (Common Usage)
1. **exitStmt** - EXIT FOR, EXIT FUNCTION critical for control flow
2. **doLoopStmt** - DO...LOOP WHILE/UNTIL very common
3. **whileWendStmt** - WHILE...WEND loops
4. **onErrorStmt** - Error handling essential for robust code
5. **redimStmt** - Dynamic arrays common

### Medium Priority
1. **openStmt/closeStmt** - File I/O basics
2. **inputStmt/writeStmt** - File I/O operations
3. **goToStmt** - Legacy code support
4. **withStmt** - WITH blocks for cleaner object code
5. **eraseStmt** - Array cleanup

### Low Priority
1. File system operations (can use .NET equivalents)
2. Registry operations (Windows-specific)
3. GOSUB/RETURN (legacy BASIC)
4. Property procedures (can convert to regular methods)
5. String positioning (LSET/RSET)

### Can Skip
1. GUI-specific statements (replaced by GUI framework)
2. SENDKEYS, BEEP (platform-specific)
3. Preprocessor macros (already handled with file rejection)
4. DEFTYPE statements (explicit typing preferred)

---

## Notes

- **blockStmt** is a container that includes all other statements
- **dictionaryCallStmt** is for special dictionary access syntax
- Many statements can have simple conversions (1:1 mapping to C#)
- Some statements require context-aware conversion
- GUI-related statements will use the GUI converter framework
- File I/O statements should convert to System.IO equivalents
