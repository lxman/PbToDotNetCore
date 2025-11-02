namespace PbToDotNetCore.Models;

/// <summary>
/// Base class for all expressions
/// </summary>
public abstract class Expression
{
}

/// <summary>
/// Literal value: "text", 123, true, etc.
/// </summary>
public class LiteralExpression : Expression
{
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Variable/identifier reference: x, myVar, etc.
/// </summary>
public class IdentifierExpression : Expression
{
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Binary operation: x + y, a == b, etc.
/// </summary>
public class BinaryExpression : Expression
{
    public Expression Left { get; set; } = new LiteralExpression();
    public string Operator { get; set; } = string.Empty;
    public Expression Right { get; set; } = new LiteralExpression();
}

/// <summary>
/// Unary operation: -x, !flag, etc.
/// </summary>
public class UnaryExpression : Expression
{
    public string Operator { get; set; } = string.Empty;
    public Expression Operand { get; set; } = new LiteralExpression();
}

/// <summary>
/// Method/function call expression: obj.Method(), Function(args)
/// </summary>
public class CallExpression : Expression
{
    public Expression? Target { get; set; }  // null for simple function calls
    public string MethodName { get; set; } = string.Empty;
    public List<Expression> Arguments { get; set; } = [];
}

/// <summary>
/// Member access: obj.Property
/// </summary>
public class MemberAccessExpression : Expression
{
    public Expression Target { get; set; } = new IdentifierExpression();
    public string MemberName { get; set; } = string.Empty;
}

/// <summary>
/// Array access: arr[index]
/// </summary>
public class ArrayAccessExpression : Expression
{
    public Expression Array { get; set; } = new IdentifierExpression();
    public Expression Index { get; set; } = new LiteralExpression();
}

/// <summary>
/// Raw code expression (for unparsed/unhandled expressions)
/// </summary>
public class RawCodeExpression : Expression
{
    public string Code { get; set; } = string.Empty;
}
