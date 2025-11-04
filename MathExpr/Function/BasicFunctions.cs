namespace MathExpr.Function;

public static class BasicFunctions
{
	public static IExpr Sqrt(IExpr expr)
		=> new Sqrt(expr);
	public static IExpr Exp(IExpr expr)
		=> new Exp(expr);
	public static IExpr Log(IExpr expr)
		=> new Log(expr);
	public static IExpr Sinh(IExpr expr)
		=> new Sinh(expr);
	public static IExpr Cosh(IExpr expr)
		=> new Cosh(expr);
	public static IExpr Tanh(IExpr expr)
		=> new Tanh(expr);

	public static IExpr Sqrt(double value)
		=> new Sqrt(new Constant(value));
	public static IExpr Exp(double value)
		=> new Exp(new Constant(value));
	public static IExpr Log(double value)
		=> new Log(new Constant(value));
	public static IExpr Sinh(double value)
		=> new Sinh(new Constant(value));
	public static IExpr Cosh(double value)
		=> new Cosh(new Constant(value));
	public static IExpr Tanh(double value)
		=> new Tanh(new Constant(value));
}

public sealed record Sqrt(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Sqrt(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Exp(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Exp(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Log(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Log(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Sinh(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Sinh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Cosh(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Cosh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record Tanh(IExpr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Tanh(value);
	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
