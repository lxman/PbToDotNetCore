using PbToDotNetCore.Models;
using System.Text;

namespace PbToDotNetCore.Generator;

/// <summary>
/// Generates C# code from the IR model
/// </summary>
public class CSharpGenerator
{
    private int _indentLevel = 0;
    private string Indent => new(' ', _indentLevel * 4);

    public string Generate(CodeModel model)
    {
        var sb = new StringBuilder();

        // Using directives
        foreach (var usingDirective in model.UsingDirectives)
        {
            sb.AppendLine($"using {usingDirective};");
        }
        sb.AppendLine();

        // Module class wrapper
        sb.AppendLine($"public class {model.ModuleName}");
        sb.AppendLine("{");
        _indentLevel++;

        // Interfaces
        foreach (var iface in model.Interfaces)
        {
            sb.Append(GenerateInterface(iface));
        }

        // Classes
        foreach (var cls in model.Classes)
        {
            sb.Append(GenerateClass(cls));
        }

        // Functions
        foreach (var func in model.Functions)
        {
            sb.Append(GenerateFunction(func));
        }

        _indentLevel--;
        sb.AppendLine("}");

        return sb.ToString();
    }

    private string GenerateInterface(InterfaceDeclaration iface)
    {
        var sb = new StringBuilder();

        sb.Append($"{Indent}{iface.Visibility} interface {iface.Name}");

        if (iface.InheritedInterfaces.Count > 0)
        {
            sb.Append($" : {string.Join(", ", iface.InheritedInterfaces)}");
        }

        sb.AppendLine();
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        foreach (var method in iface.Methods)
        {
            var parameters = string.Join(", ", method.Parameters.Select(p =>
                $"{(p.IsByRef ? "ref " : string.Empty)}{p.Type} {p.Name}"));
            sb.AppendLine($"{Indent}{method.ReturnType} {method.Name}({parameters});");
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");
        sb.AppendLine();

        return sb.ToString();
    }

    private string GenerateClass(ClassDeclaration cls)
    {
        var sb = new StringBuilder();

        sb.Append($"{Indent}{cls.Visibility} class {cls.Name}");

        if (cls.ImplementedInterfaces.Count > 0)
        {
            sb.Append($" : {string.Join(", ", cls.ImplementedInterfaces)}");
        }

        sb.AppendLine();
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        // Fields
        foreach (var field in cls.Fields)
        {
            sb.AppendLine($"{Indent}{field.Visibility} {field.Type} {field.Name};");
        }

        if (cls.Fields.Count > 0)
        {
            sb.AppendLine();
        }

        // Methods
        foreach (var method in cls.Methods)
        {
            sb.Append(GenerateMethod(method));
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");
        sb.AppendLine();

        return sb.ToString();
    }

    private string GenerateMethod(MethodDeclaration method)
    {
        var sb = new StringBuilder();

        var parameters = string.Join(", ", method.Parameters.Select(p =>
            $"{(p.IsByRef ? "ref " : string.Empty)}{p.Type} {p.Name}"));

        sb.AppendLine($"{Indent}{method.Visibility} {method.ReturnType} {method.Name}({parameters})");
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        foreach (var stmt in method.Body)
        {
            sb.Append(GenerateStatement(stmt));
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");
        sb.AppendLine();

        return sb.ToString();
    }

    private string GenerateFunction(FunctionDeclaration func)
    {
        var sb = new StringBuilder();

        var parameters = string.Join(", ", func.Parameters.Select(p =>
            $"{(p.IsByRef ? "ref " : string.Empty)}{p.Type} {p.Name}"));

        var modifiers = func.Visibility;
        if (func.IsStatic) modifiers += " static";

        sb.AppendLine($"{Indent}{modifiers} {func.ReturnType} {func.Name}({parameters})");
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        // Function result variable
        if (func.ReturnType != "void")
        {
            sb.AppendLine($"{Indent}{func.ReturnType} {func.Name}_result;");
        }

        // Local variables
        foreach (var localVar in func.LocalVariables)
        {
            if (localVar.IsArray)
            {
                sb.AppendLine($"{Indent}{localVar.Type}[] {localVar.Name} = new {localVar.Type}[{localVar.ArraySize}];");
            }
            else
            {
                sb.AppendLine($"{Indent}{localVar.Type} {localVar.Name};");
            }
        }

        // Statements
        foreach (var stmt in func.Body)
        {
            sb.Append(GenerateStatement(stmt));
        }

        // Return statement
        if (func.ReturnType != "void")
        {
            sb.AppendLine($"{Indent}return {func.Name}_result;");
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");

        return sb.ToString();
    }

    private string GenerateStatement(Statement stmt)
    {
        return stmt switch
        {
            AssignmentStatement assignment => GenerateAssignment(assignment),
            ReturnStatement returnStmt => GenerateReturn(returnStmt),
            CallStatement call => GenerateCallStatement(call),
            IfStatement ifStmt => GenerateIf(ifStmt),
            ForStatement forStmt => GenerateFor(forStmt),
            SwitchStatement switchStmt => GenerateSwitch(switchStmt),
            IncrementStatement incr => $"{Indent}{incr.Variable}++;\n",
            DecrementStatement decr => $"{Indent}{decr.Variable}--;\n",
            RawCodeStatement raw => $"{Indent}{raw.Code}\n",
            _ => $"{Indent}// Unhandled statement: {stmt.GetType().Name}\n"
        };
    }

    private string GenerateAssignment(AssignmentStatement assignment)
    {
        var value = GenerateExpression(assignment.Value);
        return $"{Indent}{assignment.Target} = {value};\n";
    }

    private string GenerateReturn(ReturnStatement returnStmt)
    {
        if (returnStmt.Value == null)
        {
            return $"{Indent}return;\n";
        }

        var value = GenerateExpression(returnStmt.Value);
        return $"{Indent}return {value};\n";
    }

    private string GenerateCallStatement(CallStatement call)
    {
        var args = string.Join(", ", call.Arguments.Select(GenerateExpression));

        if (call.Target != null)
        {
            var target = GenerateExpression(call.Target);
            return $"{Indent}{target}.{call.MethodName}({args});\n";
        }
        else
        {
            // Map common PowerBASIC functions to C# equivalents
            var csCall = call.MethodName.ToUpper() switch
            {
                "MSGBOX" => $"{Indent}MessageBox.Show({args});\n",
                "PRINT" => $"{Indent}Console.WriteLine({args});\n",
                _ => $"{Indent}{call.MethodName}({args});\n"
            };
            return csCall;
        }
    }

    private string GenerateIf(IfStatement ifStmt)
    {
        var sb = new StringBuilder();
        var condition = GenerateExpression(ifStmt.Condition);

        sb.AppendLine($"{Indent}if ({condition})");
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        foreach (var stmt in ifStmt.ThenBlock)
        {
            sb.Append(GenerateStatement(stmt));
        }

        _indentLevel--;
        sb.Append($"{Indent}}}");

        if (ifStmt.ElseBlock.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"{Indent}else");
            sb.AppendLine($"{Indent}{{");
            _indentLevel++;

            foreach (var stmt in ifStmt.ElseBlock)
            {
                sb.Append(GenerateStatement(stmt));
            }

            _indentLevel--;
            sb.Append($"{Indent}}}");
        }

        sb.AppendLine();
        return sb.ToString();
    }

    private string GenerateFor(ForStatement forStmt)
    {
        var sb = new StringBuilder();
        var start = GenerateExpression(forStmt.StartValue);
        var end = GenerateExpression(forStmt.EndValue);
        var step = GenerateExpression(forStmt.StepValue);

        sb.AppendLine($"{Indent}for ({forStmt.LoopVariable} = {start}; {forStmt.LoopVariable} <= {end}; {forStmt.LoopVariable} += {step})");
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        foreach (var stmt in forStmt.Body)
        {
            sb.Append(GenerateStatement(stmt));
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");

        return sb.ToString();
    }

    private string GenerateSwitch(SwitchStatement switchStmt)
    {
        var sb = new StringBuilder();
        var expr = GenerateExpression(switchStmt.Expression);

        sb.AppendLine($"{Indent}switch ({expr})");
        sb.AppendLine($"{Indent}{{");
        _indentLevel++;

        foreach (var caseItem in switchStmt.Cases)
        {
            var caseValue = GenerateExpression(caseItem.CaseValue);
            sb.AppendLine($"{Indent}case {caseValue}:");
            _indentLevel++;

            foreach (var stmt in caseItem.Statements)
            {
                sb.Append(GenerateStatement(stmt));
            }

            sb.AppendLine($"{Indent}break;");
            _indentLevel--;
        }

        if (switchStmt.DefaultCase != null)
        {
            sb.AppendLine($"{Indent}default:");
            _indentLevel++;

            foreach (var stmt in switchStmt.DefaultCase)
            {
                sb.Append(GenerateStatement(stmt));
            }

            sb.AppendLine($"{Indent}break;");
            _indentLevel--;
        }

        _indentLevel--;
        sb.AppendLine($"{Indent}}}");

        return sb.ToString();
    }

    private string GenerateExpression(Expression expr)
    {
        return expr switch
        {
            LiteralExpression literal => literal.Value,
            IdentifierExpression identifier => identifier.Name,
            BinaryExpression binary => $"{GenerateExpression(binary.Left)} {binary.Operator} {GenerateExpression(binary.Right)}",
            UnaryExpression unary => $"{unary.Operator}{GenerateExpression(unary.Operand)}",
            CallExpression call => GenerateCallExpression(call),
            MemberAccessExpression member => $"{GenerateExpression(member.Target)}.{member.MemberName}",
            ArrayAccessExpression array => $"{GenerateExpression(array.Array)}[{GenerateExpression(array.Index)}]",
            RawCodeExpression raw => raw.Code,
            _ => $"/* Unhandled expression: {expr.GetType().Name} */"
        };
    }

    private string GenerateCallExpression(CallExpression call)
    {
        var args = string.Join(", ", call.Arguments.Select(GenerateExpression));

        if (call.Target != null)
        {
            var target = GenerateExpression(call.Target);
            return $"{target}.{call.MethodName}({args})";
        }
        else
        {
            // Map common PowerBASIC functions to C# equivalents
            return call.MethodName.ToUpper() switch
            {
                "FORMAT$" => $"{args}.ToString()",
                _ => $"{call.MethodName}({args})"
            };
        }
    }
}
