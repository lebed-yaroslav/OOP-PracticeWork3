namespace MathExpr.Functions;

public static class BasicFunctions
{
	public static Expr Sqrt(Expr expr)
		=> new Sqrt(expr);
	public static Expr Exp(Expr expr)
		=> new Exp(expr);
	public static Expr Log(Expr expr)
		=> new Log(expr);
	public static Expr Sinh(Expr expr)
		=> new Sinh(expr);
	public static Expr Cosh(Expr expr)
		=> new Cosh(expr);
	public static Expr Tanh(Expr expr)
		=> new Tanh(expr);
}

public sealed record Sqrt(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Sqrt(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Exp(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Exp(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Log(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Log(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Sinh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Sinh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Cosh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Cosh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Tanh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Tanh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
