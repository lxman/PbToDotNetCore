using PbToDotNetCore.Parser;

namespace PbToDotNetCore;

public class PbToCSharpConverter : PowerBasicBaseVisitor<string>
{
    private int _indentLevel = 0;
    private string Indent => new string(' ', _indentLevel * 4);
    private string? _currentFunctionName = null;
    private readonly HashSet<string> _arrayNames = [];
    private readonly Dictionary<string, string> _variableTypes = [];
    
    // Root-level visitors
    public override string VisitStartRule(PowerBasicParser.StartRuleContext context)
    {
        // Entry point - visits the module
        return Visit(context.module());
    }

    public override string VisitModule(PowerBasicParser.ModuleContext context)
    {
        var result = "";

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
        return context.moduleBodyElement().Aggregate("", (current, element) => current + Visit(element));
    }

    public override string VisitModuleBodyElement(PowerBasicParser.ModuleBodyElementContext context)
    {
        // This delegates to the specific element type
        return VisitChildren(context);
    }

    // Block visitors (for function bodies)
    public override string VisitBlock(PowerBasicParser.BlockContext context)
    {
        // Visit all statements in the block
        return context.blockStmt().Aggregate("", (current, stmt) => current + Visit(stmt));
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

        // In PowerBASIC, assigning to function name sets return value
        // Replace with _result variable name
        if (_currentFunctionName is not null && leftSide == _currentFunctionName)
        {
            leftSide = $"{_currentFunctionName}_result";
        }

        string? rightSide = Visit(context.valueStmt());

        // Add type suffix to numeric literals if needed
        if (_variableTypes.TryGetValue(leftSide, out string? varType))
        {
            rightSide = AddNumericSuffix(rightSide, varType);
        }

        return $"{Indent}{leftSide} = {rightSide};\n";
    }
    
    // Expression/value visitors
    // NOTE: valueStmt has labeled alternatives, so we handle specific types
    public override string VisitVsLiteral(PowerBasicParser.VsLiteralContext context)
    {
        // Literal values (numbers, strings, etc.)
        return context.GetText();
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

    public override string VisitICS_S_ProcedureOrArrayCall(PowerBasicParser.ICS_S_ProcedureOrArrayCallContext context)
    {
        // Handle function calls and array indexing in statements/expressions
        string? funcName = context.ambiguousIdentifier()?.GetText();
        string? typeHint = context.typeHint()?.GetText();  // $ for string functions

        // Get arguments - argsCall() returns an array, we want the first one if it exists
        var args = "";
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

        // Process subroutine parameters
        var parameters = "";
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
                var modifier = "";
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

        return result;
    }

    public override string VisitFunctionStmt(PowerBasicParser.FunctionStmtContext context)
    {
        string? functionName = context.ambiguousIdentifier()?.GetText();
        string returnType = ConvertType(context.asTypeClause()?.GetText());

        // Process function parameters
        var parameters = "";
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
                var modifier = "";
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

        // In PowerBASIC, the function name is used as a return value variable
        // Create a local variable with same name to hold the return value
        result += $"{Indent}{returnType} {functionName}_result;\n";

        // Visit function body
        PowerBasicParser.BlockContext? block = context.block();
        if (block is not null)
        {
            result += Visit(block);
        }

        // Return the result variable
        result += $"{Indent}return {functionName}_result;\n";

        _indentLevel--;
        _currentFunctionName = null;

        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitVariableStmt(PowerBasicParser.VariableStmtContext context)
    {
        var result = "";
        PowerBasicParser.VariableListStmtContext? varList = context.variableListStmt();

        if (varList is null) return result;
        foreach (PowerBasicParser.VariableSubStmtContext? varSub in varList.variableSubStmt())
        {
            string? varName = varSub.ambiguousIdentifier()?.GetText();
            string varType = ConvertType(varSub.asTypeClause()?.GetText());

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
                        .Select(dimension => dimension ?? "").ToList();

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
        var result = "";

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
            .Aggregate("", (current, caseValue) => current + $"{Indent}case {caseValue}:\n");
    }

    public override string VisitCaseCondExprValue(PowerBasicParser.CaseCondExprValueContext context)
    {
        // Simple value case: CASE 1, CASE "Monday", etc.
        return Visit(context.valueStmt());
    }

    // IF statement visitors
    public override string VisitBlockIfThenElse(PowerBasicParser.BlockIfThenElseContext context)
    {
        var result = "";

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

        var result = $"{Indent}for ({loopVar} = {startValue}; {loopVar} <= {endValue}; {loopVar} += {stepValue})\n";
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

    // CLASS/INTERFACE/METHOD visitors
    public override string VisitClassStmt(PowerBasicParser.ClassStmtContext context)
    {
        string? className = context.ambiguousIdentifier()?.GetText();
        var result = $"{Indent}public class {className}\n";
        result += $"{Indent}{{\n";

        _indentLevel++;

        // Visit all class body elements (interfaces, event sources)
        result = context.classBodyElement().Aggregate(result, (current, element) => current + Visit(element));

        _indentLevel--;
        result += $"{Indent}}}\n";

        return result;
    }

    public override string VisitClassBodyElement(PowerBasicParser.ClassBodyElementContext context)
    {
        // Delegates to specific element types
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

        // For interface methods, we just declare the signature
        // PowerBASIC allows methods with bodies in interfaces, but C# doesn't (until C# 8+)
        var result = $"{Indent}{returnType} {methodName}();\n";

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
        var args = "";
        if (context.argsCall() is not null)
        {
            args = Visit(context.argsCall());
        }

        return $"{Indent}{procName}({args});\n";
    }

    // Procedure call visitors
    public override string VisitImplicitCallStmt_InBlock(PowerBasicParser.ImplicitCallStmt_InBlockContext context)
    {
        // Handle implicit procedure calls (like MSGBOX "text")
        return VisitChildren(context);
    }

    public override string VisitICS_B_ProcedureCall(PowerBasicParser.ICS_B_ProcedureCallContext context)
    {
        // Get the procedure name
        string? procName = context.certainIdentifier()?.GetText();

        // Get the arguments
        var args = "";
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

    private static string ConvertType(string? pbType)
    {
        return pbType?.ToUpper() switch
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

            _ => "object"
        };
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
