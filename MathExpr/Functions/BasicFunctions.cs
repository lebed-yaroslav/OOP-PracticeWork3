namespace MathExpr.Functions;
using static BasicFunctions;


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
	public override Expr Derivative()
		=> 0.5 / this;
}

public sealed record Exp(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Exp(value);
	public override Expr Derivative()
		=> this;
}

public sealed record Log(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Log(value);
	public override Expr Derivative()
		=> 1 / Argument;
}

public sealed record Sinh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Sinh(value);
	public override Expr Derivative()
		=> Cosh(Argument);
}


public sealed record Cosh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Cosh(value);
	public override Expr Derivative()
		=> Sinh(Argument);
}

public sealed record Tanh(Expr Argument) : Function(Argument)
{
	public override double ComputeFor(double value)
		=> Math.Tanh(value);
	public override Expr Derivative()
		=> 1 - (this ^ 2);
}
