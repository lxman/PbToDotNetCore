namespace PbToDotNetCore.Models;

/// <summary>
/// Base class for all statements
/// </summary>
public abstract class Statement
{
}

/// <summary>
/// Assignment statement: x = value
/// </summary>
public class AssignmentStatement : Statement
{
    public string Target { get; set; } = string.Empty;
    public Expression Value { get; set; } = new LiteralExpression();
}

/// <summary>
/// Return statement: return value
/// </summary>
public class ReturnStatement : Statement
{
    public Expression? Value { get; set; }
}

/// <summary>
/// Method/function call statement: obj.Method() or Function()
/// </summary>
public class CallStatement : Statement
{
    public Expression? Target { get; set; }  // null for simple function calls
    public string MethodName { get; set; } = string.Empty;
    public List<Expression> Arguments { get; set; } = [];
}

/// <summary>
/// If statement
/// </summary>
public class IfStatement : Statement
{
    public Expression Condition { get; set; } = new LiteralExpression();
    public List<Statement> ThenBlock { get; set; } = [];
    public List<Statement> ElseBlock { get; set; } = [];
}

/// <summary>
/// For loop statement
/// </summary>
public class ForStatement : Statement
{
    public string LoopVariable { get; set; } = string.Empty;
    public Expression StartValue { get; set; } = new LiteralExpression();
    public Expression EndValue { get; set; } = new LiteralExpression();
    public Expression StepValue { get; set; } = new LiteralExpression { Value = "1" };
    public List<Statement> Body { get; set; } = [];
}

/// <summary>
/// Switch/Select Case statement
/// </summary>
public class SwitchStatement : Statement
{
    public Expression Expression { get; set; } = new LiteralExpression();
    public List<SwitchCase> Cases { get; set; } = [];
    public List<Statement>? DefaultCase { get; set; }
}

public class SwitchCase
{
    public Expression CaseValue { get; set; } = new LiteralExpression();
    public List<Statement> Statements { get; set; } = [];
}

/// <summary>
/// Increment statement: x++
/// </summary>
public class IncrementStatement : Statement
{
    public string Variable { get; set; } = string.Empty;
}

/// <summary>
/// Decrement statement: x--
/// </summary>
public class DecrementStatement : Statement
{
    public string Variable { get; set; } = string.Empty;
}

/// <summary>
/// Raw code statement (for unparsed/unhandled constructs)
/// </summary>
public class RawCodeStatement : Statement
{
    public string Code { get; set; } = string.Empty;
}
