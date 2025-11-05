using MathExpr.Operator;
using System.Collections.Immutable;

namespace MathExpr;


public interface IExpr
{
	IEnumerable<string> Variables { get; }
	int? PolynomialDegree { get; }
	double Compute(IReadOnlyDictionary<string, double> variables);
	T Accept<T>(IExprVisitor<T> visitor);
}


public static class IExprExt
{
	extension(IExpr expr)
	{
		public bool IsConstant => !expr.Variables.Any();
		public bool IsPolynomial => expr.PolynomialDegree.HasValue;

		/// <summary>
		/// Computes value if underlying expression is constant
		/// </summary>
		/// <exception cref="KeyNotFoundException"/>
		/// <returns>Computed value</returns>
		public double ComputeConstant() => expr.Compute(ImmutableDictionary<string, double>.Empty);
	}
}


public abstract record Expr : IExpr
{
	public abstract IEnumerable<string> Variables { get; }
	public abstract int? PolynomialDegree { get; }
	public abstract double Compute(IReadOnlyDictionary<string, double> variables);
	public abstract T Accept<T>(IExprVisitor<T> visitor);


	// Operators:
	public static implicit operator Expr(double value)
		=> new Constant(value);

	public static Expr operator -(Expr operand)
		=> new Negate(operand);

	public static Expr operator +(Expr left, Expr right)
		=> new Add(left, right);
	public static Expr operator -(Expr left, Expr right)
		=> new Subtract(left, right);
	public static Expr operator *(Expr left, Expr right)
		=> new Multiply(left, right);
	public static Expr operator /(Expr left, Expr right)
		=> new Divide(left, right);
	public static Expr operator ^(Expr left, Expr right)
		=> new Power(left, right);
}
