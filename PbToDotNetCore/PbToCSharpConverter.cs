using PbToDotNetCore.Parser;
using PbToDotNetCore.Gui;

namespace PbToDotNetCore;

// Helper class to store method information
internal class MethodInfo
{
    public string Name { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public List<(string Modifier, string Type, string Name)> Parameters { get; set; } = [];
    public PowerBasicParser.BlockContext? Body { get; set; }
}

// Helper class to store interface information
internal class InterfaceInfo
{
    public string Name { get; set; } = string.Empty;
    public List<string> InheritedInterfaces { get; set; } = [];
    public List<MethodInfo> Methods { get; set; } = [];
}

public class PbToCSharpConverter : PowerBasicBaseVisitor<string>
{
    private int _indentLevel = 0;
    private string Indent => new(' ', _indentLevel * 4);
    private string? _currentFunctionName;
    private bool _isInVoidFunction = false; // true for SUB, false for FUNCTION
    private readonly HashSet<string> _arrayNames = [];
    private readonly Dictionary<string, string> _variableTypes = [];
    private readonly HashSet<string> _loopVariables = [];
    private readonly IGuiConverter _guiConverter;

    // Track whether we're currently processing a method body
    private bool _isInMethodBody = false;
    private string? _currentMethodReturnType = null;

    // COM base interfaces to filter out when converting to C#
    private static readonly HashSet<string> ComBaseInterfaces = new(StringComparer.OrdinalIgnoreCase)
    {
        "IUNKNOWN",
        "IDISPATCH",
        "ISTREAM",
        "ISTORAGE",
        "IMONIKER",
        "IPERSIST",
        "IPERSISTSTREAM",
        "IPERSISTSTORAGE",
        "IDATAOBJECT",
        "IDROPTARGET",
        "IDROPSOURCE"
    };

    public PbToCSharpConverter()
    {
        // Load configuration from file if not already loaded
        ConfigurationLoader.LoadConfiguration();

        _guiConverter = GuiConverterFactory.GetConverter();
    }

    // Root-level visitors
    public override string VisitStartRule(PowerBasicParser.StartRuleContext context)
    {
        // Entry point - visits the module
        return Visit(context.module());
    }

    public override string VisitModule(PowerBasicParser.ModuleContext context)
    {
        var result = string.Empty;

        // Add using statements for C#
        result += "using System;\n";
        result += "using System.Windows.Forms;\n\n";

        // Visit compiler directives (optional)
        if (context.compilerDirectives() is not null)
          result += Visit(context.compilerDirectives());

        // Create a class wrapper for the module
        result += "public class PowerBasicModule\n{\n";
        _indentLevel++;

        // Visit module body (functions, subs, etc.)
        if (context.moduleBody() is not null)
          result += Visit(context.moduleBody());

        _indentLevel--;
        result += "}\n";

        return result;
    }
    
    // Module body visitors
    public override string VisitModuleBody(PowerBasicParser.ModuleBodyContext context)
    {
        // Visit all module elements (functions, subs, types, etc.)
        return context.moduleBodyElement().Aggregate(string.Empty, (current, element) => current + Visit(element));
    }

    public override string VisitModuleBodyElement(PowerBasicParser.ModuleBodyElementContext context)
    {
        // This delegates to the specific element type
        return VisitChildren(context);
    }

    // Block visitors (for function bodies)
    public override string VisitBlock(PowerBasicParser.BlockContext context)
    {
        // Visit all statements in the block, grouping assembly and detecting THREAD CREATE...CLOSE blocks
        var result = string.Empty;
        var statements = context.blockStmt();
        var assemblyBlock = new List<string>();

        for (int i = 0; i < statements.Length; i++)
        {
            var stmt = statements[i];

            // Check if this is an assembly statement
            if (stmt.asmStmt() != null)
            {
                // Collect assembly instruction
                string asmInstruction = stmt.asmStmt().GetText().Substring(1).Trim();
                assemblyBlock.Add(asmInstruction);
            }
            // Check if this is a THREAD CREATE statement - start of thread block
            else if (stmt.threadStmt() != null && IsThreadCreate(stmt.threadStmt()))
            {
                // Flush any accumulated assembly statements first
                if (assemblyBlock.Count > 0)
                {
                    result += GenerateAssemblyBlock(assemblyBlock);
                    assemblyBlock.Clear();
                }

                // Find the matching THREAD CLOSE and collect everything in between
                var threadBlockStatements = new List<string>();
                int endIndex = i;

                // Add the CREATE statement
                threadBlockStatements.Add(GetStatementText(stmt));

                // Collect all statements until we find THREAD CLOSE
                for (int j = i + 1; j < statements.Length; j++)
                {
                    var nextStmt = statements[j];
                    threadBlockStatements.Add(GetStatementText(nextStmt));

                    if (nextStmt.threadStmt() != null && IsThreadClose(nextStmt.threadStmt()))
                    {
                        endIndex = j;
                        break;
                    }
                }

                // Generate the thread block comment
                result += GenerateThreadBlock(threadBlockStatements);

                // Skip ahead past all the statements we've processed
                i = endIndex;
            }
            else
            {
                // Flush any accumulated assembly statements
                if (assemblyBlock.Count > 0)
                {
                    result += GenerateAssemblyBlock(assemblyBlock);
                    assemblyBlock.Clear();
                }

                // Visit the statement normally
                result += Visit(stmt);
            }
        }

        // Handle any remaining assembly statements at the end
        if (assemblyBlock.Count > 0)
        {
            result += GenerateAssemblyBlock(assemblyBlock);
        }

        return result;
    }

    private bool IsThreadCreate(PowerBasicParser.ThreadStmtContext context)
    {
        string text = context.GetText().ToUpper();
        return text.Contains("THREADCREATE") || text.StartsWith("THREAD") && text.Contains("CREATE");
    }

    private bool IsThreadClose(PowerBasicParser.ThreadStmtContext context)
    {
        string text = context.GetText().ToUpper();
        return text.Contains("THREADCLOSE") || text.StartsWith("THREAD") && text.Contains("CLOSE");
    }

    private string GetStatementText(PowerBasicParser.BlockStmtContext stmt)
    {
        // Get a readable representation of the statement
        string text = stmt.GetText();

        // For readability, try to format common statements better
        if (stmt.threadStmt() != null)
        {
            return text; // Thread statements already readable
        }
        else if (stmt.explicitCallStmt() != null || stmt.implicitCallStmt_InBlock() != null)
        {
            // Function calls like MSGBOX
            return text;
        }
        else
        {
            return text;
        }
    }

    private string GenerateAssemblyBlock(List<string> assemblyInstructions)
    {
        var result = $"{Indent}// ============================================================\n";
        result += $"{Indent}// INLINE ASSEMBLY DETECTED - MANUAL CONVERSION REQUIRED\n";
        result += $"{Indent}// PowerBASIC inline assembly cannot be automatically converted.\n";
        result += $"{Indent}// Original assembly code:\n";

        foreach (var instruction in assemblyInstructions)
        {
            result += $"{Indent}//   ! {instruction}\n";
        }

        result += $"{Indent}// Consider: C# arithmetic operations, System.Runtime.Intrinsics, or P/Invoke\n";
        result += $"{Indent}// ============================================================\n";
        result += $"{Indent}throw new NotImplementedException(\"Inline assembly block with {assemblyInstructions.Count} instruction(s)\");\n";

        return result;
    }

    private string GenerateThreadBlock(List<string> threadBlockStatements)
    {
        var result = $"{Indent}// ============================================================\n";
        result += $"{Indent}// THREAD BLOCK DETECTED - MANUAL CONVERSION REQUIRED\n";
        result += $"{Indent}// PowerBASIC THREAD CREATE...CLOSE block with {threadBlockStatements.Count} statement(s)\n";
        result += $"{Indent}// \n";
        result += $"{Indent}// Original PowerBASIC code:\n";

        foreach (var stmt in threadBlockStatements)
        {
            result += $"{Indent}//   {stmt}\n";
        }

        result += $"{Indent}// \n";
        result += $"{Indent}// Conversion Guide:\n";
        result += $"{Indent}// - Replace THREAD CREATE func TO handle with: thread = new Thread(() => func()); thread.Start();\n";
        result += $"{Indent}// - Replace THREAD WAIT handle with: thread.Join();\n";
        result += $"{Indent}// - Remove THREAD CLOSE handle (automatic cleanup in C#)\n";
        result += $"{Indent}// - Convert other statements within the block as needed\n";
        result += $"{Indent}// ============================================================\n";
        result += $"{Indent}throw new NotImplementedException(\"Thread block requires manual conversion\");\n";

        return result;
    }

    public override string VisitBlockStmt(PowerBasicParser.BlockStmtContext context)
    {
        // Delegates to specific statement types
        return VisitChildren(context);
    }

    // Assignment statement (for "x = 10" and "TestFunction = x")
    public override string VisitLetStmt(PowerBasicParser.LetStmtContext context)
    {
        string? leftSide = Visit(context.implicitCallStmt_InStmt());

        // In PowerBASIC methods, "METHOD = value" sets the return value
        if (_isInMethodBody && leftSide?.ToUpper() == "METHOD")
        {
            string? rightSide = Visit(context.valueStmt());

            // Add type suffix to numeric literals if needed
            if (_currentMethodReturnType != null && _currentMethodReturnType != "void")
            {
                rightSide = AddNumericSuffix(rightSide, _currentMethodReturnType);
            }

            return $"{Indent}return {rightSide};\n";
        }

        // In PowerBASIC, assigning to function name sets return value
        // Replace with _result variable name
        if (_currentFunctionName is not null && leftSide == _currentFunctionName)
        {
            leftSide = $"{_currentFunctionName}_result";
        }

        string? rightSide2 = Visit(context.valueStmt());

        // Add type suffix to numeric literals if needed
        if (leftSide != null && _variableTypes.TryGetValue(leftSide, out string? varType))
        {
            rightSide2 = AddNumericSuffix(rightSide2, varType);
        }

        return $"{Indent}{leftSide} = {rightSide2};\n";
    }
    
    // Expression/value visitors
    // NOTE: valueStmt has labeled alternatives, so we handle specific types
    public override string VisitVsLiteral(PowerBasicParser.VsLiteralContext context)
    {
        // Literal values (numbers, strings, etc.)
        string literalText = context.GetText();

        // Convert PowerBASIC binary literals (%xxxxxxxx) to C# binary literals (0bxxxxxxxx)
        if (literalText.StartsWith("%"))
        {
            return "0b" + literalText.Substring(1);
        }

        return literalText;
    }

    public override string VisitVsBuiltInConstant(PowerBasicParser.VsBuiltInConstantContext context)
    {
        // PowerBASIC built-in constants like $CRLF, $CR, $LF, $TAB, etc.
        string? constantName = context.ambiguousIdentifier()?.GetText()?.ToUpper();

        return constantName switch
        {
            "CRLF" => "Environment.NewLine",
            "CR" => "\"\\r\"",
            "LF" => "\"\\n\"",
            "TAB" => "\"\\t\"",
            "NUL" => "\"\\0\"",
            "DQ" => "\"\\\"\"",
            "SQ" => "\"'\"",
            _ => $"/* ${constantName} */"  // Unknown constant, leave as comment
        };
    }

    public override string VisitVsClassInstantiation(PowerBasicParser.VsClassInstantiationContext context)
    {
        // PowerBASIC: CLASS "ClassName" → C#: new ClassName()
        // The className will be a string literal
        string? className = Visit(context.valueStmt());

        // Remove quotes from the string literal
        if (className != null && className.StartsWith("\"") && className.EndsWith("\""))
        {
            className = className.Substring(1, className.Length - 2);
        }

        return $"new {className}()";
    }

    public override string VisitVsICS(PowerBasicParser.VsICSContext context)
    {
        // VsICS = implicitCallStmt (variable reference, function call)
        // Delegate to child visitor to handle function calls like FORMAT$
        return Visit(context.implicitCallStmt_InStmt());
    }

    // Binary operators
    public override string VisitVsAdd(PowerBasicParser.VsAddContext context)
    {
        // Addition: left + right
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} + {right}";
    }

    public override string VisitVsMinus(PowerBasicParser.VsMinusContext context)
    {
        // Subtraction: left - right
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} - {right}";
    }

    public override string VisitVsMult(PowerBasicParser.VsMultContext context)
    {
        // Multiplication: left * right
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} * {right}";
    }

    public override string VisitVsDiv(PowerBasicParser.VsDivContext context)
    {
        // Division: left / right
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} / {right}";
    }

    public override string VisitVsAmp(PowerBasicParser.VsAmpContext context)
    {
        // String concatenation: left & right
        // In C#, use + for string concatenation
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} + {right}";
    }

    // Comparison operators
    public override string VisitVsEq(PowerBasicParser.VsEqContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} == {right}";
    }

    public override string VisitVsNeq(PowerBasicParser.VsNeqContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} != {right}";
    }

    public override string VisitVsLt(PowerBasicParser.VsLtContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} < {right}";
    }

    public override string VisitVsGt(PowerBasicParser.VsGtContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} > {right}";
    }

    public override string VisitVsLeq(PowerBasicParser.VsLeqContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} <= {right}";
    }

    public override string VisitVsGeq(PowerBasicParser.VsGeqContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} >= {right}";
    }

    // Logical operators
    public override string VisitVsAnd(PowerBasicParser.VsAndContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} && {right}";
    }

    public override string VisitVsOr(PowerBasicParser.VsOrContext context)
    {
        string? left = Visit(context.valueStmt(0));
        string? right = Visit(context.valueStmt(1));
        return $"{left} || {right}";
    }

    public override string VisitVsNot(PowerBasicParser.VsNotContext context)
    {
        string? expr = Visit(context.valueStmt());
        return $"!{expr}";
    }

    public override string VisitImplicitCallStmt_InStmt(PowerBasicParser.ImplicitCallStmt_InStmtContext context)
    {
        // Delegate to child visitor
        return VisitChildren(context);
    }

    public override string VisitICS_S_MembersCall(PowerBasicParser.ICS_S_MembersCallContext context)
    {
        // Handle member access like obj.member or obj.method()
        // Format: (variable)? memberCall+
        var result = string.Empty;

        // Get the base object/variable if present
        if (context.iCS_S_VariableOrProcedureCall() != null)
        {
            result = Visit(context.iCS_S_VariableOrProcedureCall());
        }
        else if (context.iCS_S_ProcedureOrArrayCall() != null)
        {
            result = Visit(context.iCS_S_ProcedureOrArrayCall());
        }

        // Add each member call
        foreach (var memberCall in context.iCS_S_MemberCall())
        {
            result += Visit(memberCall);
        }

        // Handle dictionary call if present
        if (context.dictionaryCallStmt() != null)
        {
            result += Visit(context.dictionaryCallStmt());
        }

        return result;
    }

    public override string VisitICS_S_MemberCall(PowerBasicParser.ICS_S_MemberCallContext context)
    {
        // Handle a single member access: .member
        var result = ".";

        if (context.iCS_S_VariableOrProcedureCall() != null)
        {
            result += Visit(context.iCS_S_VariableOrProcedureCall());
        }
        else if (context.iCS_S_ProcedureOrArrayCall() != null)
        {
            result += Visit(context.iCS_S_ProcedureOrArrayCall());
        }

        return result;
    }

    public override string VisitICS_S_ProcedureOrArrayCall(PowerBasicParser.ICS_S_ProcedureOrArrayCallContext context)
    {
        // Handle function calls and array indexing in statements/expressions
        string? funcName = context.ambiguousIdentifier()?.GetText();
        string? typeHint = context.typeHint()?.GetText();  // $ for string functions

        // Get arguments - argsCall() returns an array, we want the first one if it exists
        var args = string.Empty;
        PowerBasicParser.ArgsCallContext[]? argsCallArray = context.argsCall();
        if (argsCallArray is not null && argsCallArray.Length > 0)
        {
            List<string> argList =
                argsCallArray[0]
                    .argCall()
                    .Select(Visit)
                    .Where(argValue => !string.IsNullOrEmpty(argValue))
                    .ToList();
            args = string.Join(", ", argList);
        }

        // Check if this is an array indexing operation
        if (funcName is not null && _arrayNames.Contains(funcName))
        {
            // Array indexing: arrayName[index]
            return $"{funcName}[{args}]";
        }

        // Handle special PowerBASIC functions
        if (funcName?.ToUpper() == "FORMAT" && typeHint == "$")
        {
            // FORMAT$(value) → value.ToString()
            // FORMAT$(value, format) → value.ToString(format)
            string[] argArray = args.Split([", "], StringSplitOptions.None);
            switch (argArray.Length)
            {
                case 1:
                    return $"{argArray[0]}.ToString()";
                case >= 2:
                    // PowerBASIC format strings are similar to .NET format strings
                    return $"{argArray[0]}.ToString({argArray[1]})";
            }
        }

        // Handle BIN$() - binary string conversion function
        if (funcName?.ToUpper() == "BIN" && typeHint == "$")
        {
            // BIN$(value) → Convert.ToString(value, 2)
            return $"Convert.ToString({args}, 2)";
        }

        // Handle VARPTR() - pointer function
        if (funcName?.ToUpper() == "VARPTR")
        {
            // VARPTR() cannot be directly converted - return placeholder
            return $"IntPtr.Zero /* TODO: VARPTR({args}) - Consider Marshal.AllocHGlobal or GCHandle.Alloc */";
        }

        // Default: regular function call
        string fullName = funcName + typeHint;
        return $"{fullName}({args})";
    }

    public override string VisitICS_S_VariableOrProcedureCall(PowerBasicParser.ICS_S_VariableOrProcedureCallContext context)
    {
        // Simple variable or procedure reference
        return context.GetText();
    }

    public override string VisitSubStmt(PowerBasicParser.SubStmtContext context)
    {
        string? subName = context.ambiguousIdentifier()?.GetText();

        // Mark that we're in a void function (SUB)
        _isInVoidFunction = true;
        _currentFunctionName = subName;

        // Process subroutine parameters
        var parameters = string.Empty;
        if (context.argList() is not null)
        {
            var paramList = new List<string>();
            foreach (PowerBasicParser.ArgContext? arg in context.argList().arg())
            {
                string? paramName = arg.ambiguousIdentifier()?.GetText();
                string paramType = ConvertType(arg.asTypeClause()?.GetText());

                // Track parameter type
                if (paramName is not null)
                {
                    _variableTypes[paramName] = paramType;
                }

                // BYVAL vs BYREF - C# uses ref keyword for BYREF
                var modifier = string.Empty;
                if (arg.BYREF() is not null)
                {
                    modifier = "ref ";
                }

                paramList.Add($"{modifier}{paramType} {paramName}");
            }
            parameters = string.Join(", ", paramList);
        }

        var result = $"{Indent}public void {subName}({parameters})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit subroutine body
        PowerBasicParser.BlockContext? block = context.block();
        if (block is not null)
        {
            result += Visit(block);
        }

        _indentLevel--;

        result += $"{Indent}}}\n";

        // Reset flags
        _isInVoidFunction = false;
        _currentFunctionName = null;

        return result;
    }

    public override string VisitFunctionStmt(PowerBasicParser.FunctionStmtContext context)
    {
        string? functionName = context.ambiguousIdentifier()?.GetText();
        string returnType = ConvertType(context.asTypeClause()?.GetText());

        // Process function parameters
        var parameters = string.Empty;
        if (context.argList() is not null)
        {
            var paramList = new List<string>();
            foreach (PowerBasicParser.ArgContext? arg in context.argList().arg())
            {
                string? paramName = arg.ambiguousIdentifier()?.GetText();
                string paramType = ConvertType(arg.asTypeClause()?.GetText());

                // Track parameter type
                if (paramName is not null)
                {
                    _variableTypes[paramName] = paramType;
                }

                // BYVAL vs BYREF - C# uses ref keyword for BYREF
                var modifier = string.Empty;
                if (arg.BYREF() is not null)
                {
                    modifier = "ref ";
                }

                paramList.Add($"{modifier}{paramType} {paramName}");
            }
            parameters = string.Join(", ", paramList);
        }

        var result = $"{Indent}public {returnType} {functionName}({parameters})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Track current function name to replace assignments
        _currentFunctionName = functionName;
        _isInVoidFunction = false; // FUNCTIONs have return values

        // Scan for loop variables before processing the block
        _loopVariables.Clear();
        PowerBasicParser.BlockContext? block = context.block();
        if (block is not null)
        {
            CollectLoopVariables(block);
        }

        // In PowerBASIC, the function name is used as a return value variable
        // Create a local variable with same name to hold the return value
        // Initialize to default value to avoid C# uninitialized variable warnings
        result += $"{Indent}{returnType} {functionName}_result = default;\n";

        // Visit function body
        if (block is not null)
        {
            result += Visit(block);
        }

        // Return the result variable
        result += $"{Indent}return {functionName}_result;\n";

        _indentLevel--;
        _currentFunctionName = null;
        _loopVariables.Clear();

        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitVariableStmt(PowerBasicParser.VariableStmtContext context)
    {
        var result = string.Empty;
        PowerBasicParser.VariableListStmtContext? varList = context.variableListStmt();

        if (varList is null) return result;
        foreach (PowerBasicParser.VariableSubStmtContext? varSub in varList.variableSubStmt())
        {
            string? varName = varSub.ambiguousIdentifier()?.GetText();

            // Skip loop variables - they'll be declared inline in the FOR statement
            if (varName != null && _loopVariables.Contains(varName))
            {
                // Still track the type for later use
                string loopVarType = ConvertType(varSub.asTypeClause()?.GetText());
                _variableTypes[varName] = loopVarType;
                continue;
            }

            string varType = ConvertType(varSub.asTypeClause()?.GetText());

            // Check if this is a pointer type (has PTR suffix)
            bool isPointer = varSub.asTypeClause()?.GetText()?.ToUpper().Contains("PTR") == true;

            if (isPointer)
            {
                // Generate comment for pointer variable
                result += $"{Indent}// ============================================================\n";
                result += $"{Indent}// POINTER TYPE DETECTED - MANUAL CONVERSION REQUIRED\n";
                result += $"{Indent}// PowerBASIC pointer: {varName} AS {varSub.asTypeClause()?.GetText()}\n";
                result += $"{Indent}// Consider: IntPtr, unsafe pointers, or Span<T>/Memory<T>\n";
                result += $"{Indent}// ============================================================\n";
                result += $"{Indent}IntPtr {varName}; // TODO: Convert pointer operations\n";
            }
            else
            {
                // Track variable type for later use (e.g., adding suffixes to literals)
                if (varName is not null)
                {
                    _variableTypes[varName] = varType;
                }

                // Check if this is an array declaration (has subscripts)
                if (varSub.subscripts() is not null)
                {
                    // Track this as an array name
                    if (varName is not null)
                    {
                        _arrayNames.Add(varName);
                    }

                    // Get array dimensions
                    List<string> dimensions =
                        varSub.subscripts()
                            .subscript()
                            .Where(subscript => subscript.valueStmt() is not null)
                            .Select(subscript => subscript.valueStmt())
                            .Where(valueStmts => valueStmts.Length > 0)
                            .Select(valueStmts => Visit(valueStmts[^1]))
                            .Select(dimension => dimension ?? string.Empty).ToList();

                    // In C#, array declaration: type[] name = new type[size];
                    // PowerBASIC arrays are 0-based by default in modern versions
                    string arrayDecl = string.Join(", ", dimensions);
                    result += $"{Indent}{varType}[] {varName} = new {varType}[{arrayDecl} + 1];\n";
                }
                else
                {
                    result += $"{Indent}{varType} {varName};\n";
                }
            }
        }

        return result;
    }

    // SELECT CASE visitor
    public override string VisitSelectCaseStmt(PowerBasicParser.SelectCaseStmtContext context)
    {
        // Get the value being tested
        string? testValue = Visit(context.valueStmt());

        var result = $"{Indent}switch ({testValue})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit all CASE clauses
        result =
            context
                .sC_Case()
                .Aggregate(result, (current, caseClause) => current + Visit(caseClause));

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitSC_Case(PowerBasicParser.SC_CaseContext context)
    {
        var result = string.Empty;

        // Visit the condition to get case labels (will be handled by specific visitors)
        result += Visit(context.sC_Cond());

        _indentLevel++;

        // Visit the block of statements for this case
        if (context.block() is not null)
        {
            result += Visit(context.block());
        }

        // Add break statement
        result += $"{Indent}break;\n";

        _indentLevel--;

        return result;
    }

    public override string VisitCaseCondElse(PowerBasicParser.CaseCondElseContext context)
    {
        // CASE ELSE → default:
        return $"{Indent}default:\n";
    }

    public override string VisitCaseCondExpr(PowerBasicParser.CaseCondExprContext context)
    {
        // CASE with one or more conditions
        return context
            .sC_CondExpr()
            .Select(Visit)
            .Aggregate(string.Empty, (current, caseValue) => current + $"{Indent}case {caseValue}:\n");
    }

    public override string VisitCaseCondExprValue(PowerBasicParser.CaseCondExprValueContext context)
    {
        // Simple value case: CASE 1, CASE "Monday", etc.
        return Visit(context.valueStmt());
    }

    // IF statement visitors
    public override string VisitBlockIfThenElse(PowerBasicParser.BlockIfThenElseContext context)
    {
        var result = string.Empty;

        // Visit the IF block
        result += Visit(context.ifBlockStmt());

        // Visit all ELSEIF blocks
        result =
            context
                .ifElseIfBlockStmt()
                .Aggregate(result, (current, elseIf) => current + Visit(elseIf));

        // Visit the ELSE block if present
        if (context.ifElseBlockStmt() is not null)
        {
            result += Visit(context.ifElseBlockStmt());
        }

        return result;
    }

    public override string VisitIfBlockStmt(PowerBasicParser.IfBlockStmtContext context)
    {
        // Get the condition
        string? condition = Visit(context.ifConditionStmt());

        var result = $"{Indent}if ({condition})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit the block of statements
        if (context.block() is not null)
        {
            result += Visit(context.block());
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitIfElseIfBlockStmt(PowerBasicParser.IfElseIfBlockStmtContext context)
    {
        // Get the condition
        string? condition = Visit(context.ifConditionStmt());

        var result = $"{Indent}else if ({condition})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit the block of statements
        if (context.block() is not null)
        {
            result += Visit(context.block());
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitIfElseBlockStmt(PowerBasicParser.IfElseBlockStmtContext context)
    {
        var result = $"{Indent}else\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit the block of statements
        if (context.block() is not null)
        {
            result += Visit(context.block());
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitIfConditionStmt(PowerBasicParser.IfConditionStmtContext context)
    {
        // The condition is just a valueStmt
        return Visit(context.valueStmt());
    }

    // FOR loop visitor
    public override string VisitForNextStmt(PowerBasicParser.ForNextStmtContext context)
    {
        // Get loop variable
        string? loopVar = context.iCS_S_VariableOrProcedureCall()?.GetText();

        // Get start value
        string? startValue = Visit(context.valueStmt(0));

        // Get end value
        string? endValue = Visit(context.valueStmt(1));

        // Get step value if present (default is 1)
        string? stepValue = "1";
        if (context.valueStmt().Length > 2)
        {
            stepValue = Visit(context.valueStmt(2));
        }

        // Get the variable type from our tracking (loop variables are skipped in LOCAL processing)
        // so we always declare them inline here
        string varType = _variableTypes.TryGetValue(loopVar ?? "", out string? foundType) ? foundType : "int";

        var result = $"{Indent}for ({varType} {loopVar} = {startValue}; {loopVar} <= {endValue}; {loopVar} += {stepValue})\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit loop body
        if (context.block() is not null)
        {
            result += Visit(context.block());
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    // DO...LOOP visitor
    public override string VisitDoLoopStmt(PowerBasicParser.DoLoopStmtContext context)
    {
        string result = "";

        // Check for WHILE/UNTIL at the beginning (DO WHILE ... LOOP or DO UNTIL ... LOOP)
        bool hasWhileAtStart = context.WHILE() != null && context.valueStmt() != null;
        bool hasUntilAtStart = context.UNTIL() != null && context.valueStmt() != null;

        if (hasWhileAtStart)
        {
            // DO WHILE condition ... LOOP → while (condition) { ... }
            string? condition = Visit(context.valueStmt());
            result += $"{Indent}while ({condition})\n";
            result += $"{Indent}{{\n";
        }
        else if (hasUntilAtStart)
        {
            // DO UNTIL condition ... LOOP → while (!(condition)) { ... }
            string? condition = Visit(context.valueStmt());
            result += $"{Indent}while (!({condition}))\n";
            result += $"{Indent}{{\n";
        }
        else
        {
            // DO ... LOOP (infinite or with condition at end)
            // Convert to do { ... } while (true) or do { ... } while (condition)
            result += $"{Indent}do\n";
            result += $"{Indent}{{\n";
        }

        _indentLevel++;

        // Visit loop body
        if (context.block() != null)
        {
            result += Visit(context.block());
        }

        _indentLevel--;

        // Check if there's a condition at the end (DO ... LOOP WHILE/UNTIL condition)
        // Note: This would be in a different grammar rule, but for simple DO...LOOP,
        // we just close with while(true) or just closing brace
        if (!hasWhileAtStart && !hasUntilAtStart)
        {
            // Simple DO...LOOP becomes do { ... } while (true)
            result += $"{Indent}}} while (true);\n";
        }
        else
        {
            result += $"{Indent}}}\n";
        }

        return result;
    }

    // INCR and DECR statement visitors
    public override string VisitIncrStmt(PowerBasicParser.IncrStmtContext context)
    {
        string? varName = context.ambiguousIdentifier()?.GetText();
        return $"{Indent}{varName}++;\n";
    }

    public override string VisitDecrStmt(PowerBasicParser.DecrStmtContext context)
    {
        string? varName = context.ambiguousIdentifier()?.GetText();
        return $"{Indent}{varName}--;\n";
    }

    public override string VisitShiftStmt(PowerBasicParser.ShiftStmtContext context)
    {
        // SHIFT LEFT variable, count  -> variable <<= count
        // SHIFT RIGHT variable, count -> variable >>= count
        string? varName = context.implicitCallStmt_InStmt()?.GetText();
        string? shiftCount = Visit(context.valueStmt());

        bool isLeft = context.GetText().ToUpper().Contains("LEFT");
        string op = isLeft ? "<<=" : ">>=";

        return $"{Indent}{varName} {op} {shiftCount};\n";
    }

    public override string VisitRotateStmt(PowerBasicParser.RotateStmtContext context)
    {
        // ROTATE LEFT variable, count  -> variable = (variable << count) | (variable >> (32 - count))
        // ROTATE RIGHT variable, count -> variable = (variable >> count) | (variable << (32 - count))
        // Note: Assumes 32-bit integers (DWORD/LONG). For other sizes, manual adjustment needed.
        string? varName = context.implicitCallStmt_InStmt()?.GetText();
        string? rotateCount = Visit(context.valueStmt());

        bool isLeft = context.GetText().ToUpper().Contains("LEFT");

        if (isLeft)
        {
            return $"{Indent}{varName} = ({varName} << {rotateCount}) | ({varName} >> (32 - {rotateCount}));\n";
        }
        else
        {
            return $"{Indent}{varName} = ({varName} >> {rotateCount}) | ({varName} << (32 - {rotateCount}));\n";
        }
    }

    public override string VisitExitStmt(PowerBasicParser.ExitStmtContext context)
    {
        // EXIT FOR → break;
        // EXIT DO → break;
        // EXIT FUNCTION → return functionName_result; (if function has return value)
        // EXIT SUB → return;
        // EXIT PROPERTY → return;

        if (context.EXIT_FOR() != null)
        {
            return $"{Indent}break;\n";
        }
        else if (context.EXIT_DO() != null)
        {
            return $"{Indent}break;\n";
        }
        else if (context.EXIT_FUNCTION() != null)
        {
            // For FUNCTIONs with return value, return the result variable
            if (!_isInVoidFunction && _currentFunctionName is not null)
            {
                return $"{Indent}return {_currentFunctionName}_result;\n";
            }
            return $"{Indent}return;\n";
        }
        else if (context.EXIT_SUB() != null)
        {
            return $"{Indent}return;\n";
        }
        else if (context.EXIT_PROPERTY() != null)
        {
            return $"{Indent}return;\n";
        }

        return string.Empty;
    }

    // Note: VisitThreadStmt is not needed - thread statements are handled in VisitBlock()
    // to group consecutive THREAD statements together with a single comment block

    // Helper method to collect loop variables from FOR loops in a block
    private void CollectLoopVariables(PowerBasicParser.BlockContext block)
    {
        foreach (var stmt in block.blockStmt())
        {
            if (stmt.forNextStmt() != null)
            {
                string? loopVar = stmt.forNextStmt().iCS_S_VariableOrProcedureCall()?.GetText();
                if (loopVar != null)
                {
                    _loopVariables.Add(loopVar);
                }
            }
            // Note: Not recursively checking nested blocks for simplicity
            // If needed, FOR loops in nested blocks can be handled in future iterations
        }
    }

    // CLASS/INTERFACE/METHOD visitors
    public override string VisitClassStmt(PowerBasicParser.ClassStmtContext context)
    {
        string? className = context.ambiguousIdentifier()?.GetText();
        var result = string.Empty;

        // Collect INSTANCE variables
        var instanceVariables = new List<(string Type, string Name)>();
        foreach (var element in context.classBodyElement())
        {
            if (element.INSTANCE() is not null && element.variableListStmt() is not null)
            {
                foreach (var variableSubStmt in element.variableListStmt().variableSubStmt())
                {
                    string? varName = variableSubStmt.ambiguousIdentifier()?.GetText();
                    string varType = ConvertType(variableSubStmt.asTypeClause()?.GetText());
                    if (varName != null)
                    {
                        instanceVariables.Add((varType, varName));
                    }
                }
            }
        }

        // Collect all interfaces with their methods
        var interfaces = new List<InterfaceInfo>();
        foreach (var element in context.classBodyElement())
        {
            if (element.interfaceStmt() is not null)
            {
                var interfaceCtx = element.interfaceStmt();
                var interfaceInfo = new InterfaceInfo
                {
                    Name = interfaceCtx.ambiguousIdentifier()?.GetText() ?? string.Empty
                };

                // Collect inherited interfaces (filter out COM base interfaces)
                foreach (var bodyElement in interfaceCtx.interfaceBodyElement())
                {
                    if (bodyElement.GetText().StartsWith("INHERIT", StringComparison.OrdinalIgnoreCase))
                    {
                        string? inheritedName = bodyElement.ambiguousIdentifier()?.GetText();
                        if (inheritedName != null && !ComBaseInterfaces.Contains(inheritedName))
                        {
                            interfaceInfo.InheritedInterfaces.Add(inheritedName);
                        }
                    }
                }

                // Collect methods with their bodies
                foreach (var bodyElement in interfaceCtx.interfaceBodyElement())
                {
                    if (bodyElement.methodStmt() is not null)
                    {
                        var methodCtx = bodyElement.methodStmt();

                        // Determine return type - if no AS clause, it's a SUB (void)
                        string returnType;
                        if (methodCtx.asTypeClause() == null)
                        {
                            returnType = "void";
                        }
                        else
                        {
                            returnType = ConvertType(methodCtx.asTypeClause()?.GetText());
                        }

                        var methodInfo = new MethodInfo
                        {
                            Name = methodCtx.ambiguousIdentifier()?.GetText() ?? string.Empty,
                            ReturnType = returnType,
                            Body = methodCtx.block()
                        };

                        // Collect parameters
                        if (methodCtx.argList() is not null)
                        {
                            foreach (var arg in methodCtx.argList().arg())
                            {
                                string? paramName = arg.ambiguousIdentifier()?.GetText();
                                string paramType = ConvertType(arg.asTypeClause()?.GetText());
                                string modifier = arg.BYREF() is not null ? "ref " : string.Empty;

                                if (paramName != null)
                                {
                                    methodInfo.Parameters.Add((modifier, paramType, paramName));
                                    _variableTypes[paramName] = paramType;
                                }
                            }
                        }

                        interfaceInfo.Methods.Add(methodInfo);
                    }
                }

                interfaces.Add(interfaceInfo);
            }
        }

        // Generate standalone C# interfaces (signatures only)
        foreach (var interfaceInfo in interfaces)
        {
            result += $"{Indent}public interface {interfaceInfo.Name}";
            if (interfaceInfo.InheritedInterfaces.Count > 0)
            {
                result += $" : {string.Join(", ", interfaceInfo.InheritedInterfaces)}";
            }
            result += "\n";
            result += $"{Indent}{{\n";

            _indentLevel++;

            foreach (var method in interfaceInfo.Methods)
            {
                var parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Modifier}{p.Type} {p.Name}"));
                result += $"{Indent}{method.ReturnType} {method.Name}({parameters});\n";
            }

            _indentLevel--;
            result += $"{Indent}}}\n";
            result += "\n";
        }

        // Generate C# class that implements the interfaces
        result += $"{Indent}public class {className}";
        if (interfaces.Count > 0)
        {
            result += $" : {string.Join(", ", interfaces.Select(i => i.Name))}";
        }
        result += "\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Add instance variables as private fields
        foreach (var (type, name) in instanceVariables)
        {
            result += $"{Indent}private {type} {name};\n";
        }

        if (instanceVariables.Count > 0)
        {
            result += "\n";
        }

        // Generate method implementations
        foreach (var interfaceInfo in interfaces)
        {
            foreach (var method in interfaceInfo.Methods)
            {
                var parameters = string.Join(", ", method.Parameters.Select(p => $"{p.Modifier}{p.Type} {p.Name}"));
                result += $"{Indent}public {method.ReturnType} {method.Name}({parameters})\n";
                result += $"{Indent}{{\n";

                _indentLevel++;

                // Set method context for METHOD = value conversion
                _isInMethodBody = true;
                _currentMethodReturnType = method.ReturnType;

                // Visit method body if present
                if (method.Body is not null)
                {
                    result += Visit(method.Body);
                }

                // Restore context
                _isInMethodBody = false;
                _currentMethodReturnType = null;

                _indentLevel--;
                result += $"{Indent}}}\n";
                result += "\n";
            }
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitClassBodyElement(PowerBasicParser.ClassBodyElementContext context)
    {
        // Check if this is an INSTANCE variable declaration
        if (context.INSTANCE() is not null && context.variableListStmt() is not null)
        {
            // Handle instance variables as class fields
            var result = string.Empty;
            foreach (var variableSubStmt in context.variableListStmt().variableSubStmt())
            {
                string? varName = variableSubStmt.ambiguousIdentifier()?.GetText();
                string varType = ConvertType(variableSubStmt.asTypeClause()?.GetText());

                result += $"{Indent}private {varType} {varName};\n";
            }
            return result;
        }

        // Delegates to specific element types (interfaces, events, etc.)
        return VisitChildren(context);
    }

    public override string VisitInterfaceStmt(PowerBasicParser.InterfaceStmtContext context)
    {
        string? interfaceName = context.ambiguousIdentifier()?.GetText();

        // Check for inherited interfaces
        List<string> inheritedInterfaces =
            (from element in context.interfaceBodyElement()
                where element.GetText().StartsWith("INHERIT", StringComparison.OrdinalIgnoreCase) && element.ambiguousIdentifier()?.GetText() is not null
                select element.ambiguousIdentifier()?.GetText()).ToList();

        var result = $"{Indent}public interface {interfaceName}";
        if (inheritedInterfaces.Count > 0)
        {
            result += $" : {string.Join(", ", inheritedInterfaces)}";
        }
        result += "\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit all methods in the interface
        result =
            context
                .interfaceBodyElement()
                .Where(element => !element.GetText().StartsWith("INHERIT", StringComparison.OrdinalIgnoreCase))
                .Aggregate(result, (current, element) => current + Visit(element));

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitInterfaceBodyElement(PowerBasicParser.InterfaceBodyElementContext context)
    {
        // Delegates to specific element types (methodStmt)
        return VisitChildren(context);
    }

    public override string VisitMethodStmt(PowerBasicParser.MethodStmtContext context)
    {
        string? methodName = context.ambiguousIdentifier()?.GetText();
        string returnType = ConvertType(context.asTypeClause()?.GetText());

        // Process method parameters
        var parameters = string.Empty;
        if (context.argList() is not null)
        {
            var paramList = new List<string>();
            foreach (PowerBasicParser.ArgContext? arg in context.argList().arg())
            {
                string? paramName = arg.ambiguousIdentifier()?.GetText();
                string paramType = ConvertType(arg.asTypeClause()?.GetText());

                // Track parameter type
                if (paramName is not null)
                {
                    _variableTypes[paramName] = paramType;
                }

                // BYVAL vs BYREF - C# uses ref keyword for BYREF
                var modifier = string.Empty;
                if (arg.BYREF() is not null)
                {
                    modifier = "ref ";
                }

                paramList.Add($"{modifier}{paramType} {paramName}");
            }
            parameters = string.Join(", ", paramList);
        }

        // For interface methods, we just declare the signature
        // PowerBASIC allows methods with bodies in interfaces, but C# doesn't (until C# 8+)
        // We skip the body and only output the signature
        var result = $"{Indent}{returnType} {methodName}({parameters});\n";

        return result;
    }

    // Explicit call visitors
    public override string VisitExplicitCallStmt(PowerBasicParser.ExplicitCallStmtContext context)
    {
        // Delegate to specific call type
        return VisitChildren(context);
    }

    public override string VisitECS_ProcedureCall(PowerBasicParser.ECS_ProcedureCallContext context)
    {
        // Get the procedure name
        string? procName = context.ambiguousIdentifier()?.GetText();

        // Get the arguments
        var args = string.Empty;
        if (context.argsCall() is not null)
        {
            args = Visit(context.argsCall());
        }

        return $"{Indent}{procName}({args});\n";
    }

    // Procedure call visitors
    // Member call: obj.Method() or obj.Method(args)
    public override string VisitICS_B_MemberCall(PowerBasicParser.ICS_B_MemberCallContext context)
    {
        string? baseName = context.certainIdentifier()?.GetText();
        string? memberName = context.ambiguousIdentifier()?.GetText();

        var args = string.Empty;
        if (context.argsCall() is not null)
        {
            args = Visit(context.argsCall());
        }

        return $"{Indent}{baseName}.{memberName}({args});\n";
    }

    // Procedure call with arguments: MSGBOX "text" or MyProc arg1, arg2
    public override string VisitICS_B_ProcedureCall(PowerBasicParser.ICS_B_ProcedureCallContext context)
    {
        string? procName = context.certainIdentifier()?.GetText();

        var args = string.Empty;
        if (context.argsCall() is not null)
        {
            args = Visit(context.argsCall());
        }

        // Map PowerBASIC procedures to C# equivalents
        string? csCall = procName?.ToUpper() switch
        {
            "MSGBOX" => $"{Indent}MessageBox.Show({args});\n",
            "PRINT" => $"{Indent}Console.WriteLine({args});\n",
            _ => $"{Indent}{procName}({args});\n"
        };

        return csCall;
    }

    public override string VisitArgsCall(PowerBasicParser.ArgsCallContext context)
    {
        // Visit all argument calls and join them with commas
        List<string> args =
            context
                .argCall()
                .Select(Visit)
                .Where(argValue => !string.IsNullOrEmpty(argValue))
                .ToList();
        return string.Join(", ", args);
    }

    public override string VisitArgCall(PowerBasicParser.ArgCallContext context)
    {
        // Visit the value statement for this argument
        return context.valueStmt() is not null
            ? Visit(context.valueStmt())
            : string.Empty;
    }

    // Print statement visitor
    public override string VisitPrintStmt(PowerBasicParser.PrintStmtContext context)
    {
        // Get the value/output to print
        string? output;
        if (context.valueStmt() is not null)
        {
            output = Visit(context.valueStmt());
        }
        else if (context.outputList() is not null)
        {
            output = Visit(context.outputList());
        }
        else
        {
            output = "\"\"";
        }

        return $"{Indent}Console.WriteLine({output});\n";
    }

    // TYPE/UNION visitors
    public override string VisitTypeStmt(PowerBasicParser.TypeStmtContext context)
    {
        string? typeName = context.ambiguousIdentifier()?.GetText();

        // Check if this type contains a union
        bool hasUnion = context.typeStmt_Element()
            .Any(e => e is PowerBasicParser.TypeElement_UnionContext);

        var result = string.Empty;

        // Add StructLayout attribute if type contains union
        if (hasUnion)
        {
            result += $"{Indent}[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]\n";
        }

        result += $"{Indent}public struct {typeName}\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit all type elements (fields and unions)
        foreach (var element in context.typeStmt_Element())
        {
            result += Visit(element);
        }

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitTypeElement_Union(PowerBasicParser.TypeElement_UnionContext context)
    {
        // Union members all start at offset 0 (overlapping memory)
        var result = string.Empty;

        foreach (var field in context.typeStmt_Element_Field())
        {
            result += $"{Indent}[System.Runtime.InteropServices.FieldOffset(0)]\n";
            result += Visit(field);
        }

        return result;
    }

    public override string VisitTypeElement_Field(PowerBasicParser.TypeElement_FieldContext context)
    {
        return VisitTypeStmt_Element_Field(context.typeStmt_Element_Field());
    }

    public override string VisitTypeStmt_Element_Field(PowerBasicParser.TypeStmt_Element_FieldContext context)
    {
        string? fieldName = context.ambiguousIdentifier()?.GetText();
        string fieldType = ConvertType(context.asTypeClause()?.GetText());

        // Check if it's an array
        if (context.subscripts() is not null)
        {
            // Get array bounds
            var subscripts = context.subscripts();
            var bounds = subscripts.subscript();

            if (bounds.Length > 0)
            {
                // For simplicity, use the upper bound as size (assuming 0-based)
                var upperBound = bounds[0].GetText();
                // In PowerBASIC arrays can be 1-based, but we'll use fixed-size arrays
                return $"{Indent}[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = {upperBound})]\n" +
                       $"{Indent}public {fieldType}[] {fieldName};\n";
            }
        }

        return $"{Indent}public {fieldType} {fieldName};\n";
    }

    private static string ConvertType(string? pbType)
    {
        if (string.IsNullOrWhiteSpace(pbType))
            return "object";

        string? result = pbType.ToUpper() switch
        {
            // Standard BASIC types
            "AS INTEGER" => "short",
            "AS LONG" => "int",
            "AS SINGLE" => "float",
            "AS DOUBLE" => "double",
            "AS STRING" => "string",

            // PowerBASIC-specific types
            "AS BYTE" => "byte",
            "AS WORD" => "ushort",      // 16-bit unsigned
            "AS DWORD" => "uint",       // 32-bit unsigned
            "AS QWORD" => "ulong",      // 64-bit unsigned
            "AS QUAD" => "long",        // 64-bit signed
            "AS CURRENCY" => "decimal", // Fixed-point decimal for financial calculations
            "AS EXTENDED" => "decimal", // 80-bit float (closest match in C#)
            "AS ASCIIZ" => "string",    // null-terminated string
            "AS BIT" => "bool",         // single bit

            _ => null  // Not a built-in type
        };

        // If it's a built-in type, return the mapped value
        if (result != null)
            return result;

        // For complex/user-defined types, strip "AS " and return the type name
        return pbType.StartsWith("AS ", StringComparison.OrdinalIgnoreCase)
            ? pbType[3..].Trim()
            : "object";
    }

    private static string AddNumericSuffix(string value, string targetType)
    {
        // Check if value is a numeric literal (simple check for digits and decimal point)
        // and doesn't already have a suffix
        if (string.IsNullOrWhiteSpace(value)) return value;

        // Check if it's a numeric literal without a suffix
        bool isNumeric = value.All(c => char.IsDigit(c) || c == '.' || c == '-');
        if (!isNumeric) return value;

        // Check if it already has a suffix (f, F, m, M, d, D, etc.)
        char lastChar = value[^1];
        if (char.IsLetter(lastChar)) return value;

        // Add appropriate suffix based on target type
        return targetType switch
        {
            "float" => value + "f",
            "decimal" => value + "m",
            _ => value
        };
    }
}
