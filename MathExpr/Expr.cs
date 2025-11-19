using MathExpr.Operator;
using System.Collections.Immutable;

namespace MathExpr;


public interface IExpr
{
	IEnumerable<string> Variables { get; }
	int? PolynomialDegree { get; }
	double Compute(IReadOnlyDictionary<string, double> variables);
}


public static class IExprExt
{
	extension(IExpr expr)
	{
		public bool IsConstant => !expr.Variables.Any();
		public bool IsPolynomial => expr.PolynomialDegree.HasValue;

		/// <summary>
		/// Computes value if underlying expression is constant <see cref="get_IsConstant(IExpr)"/>
		/// </summary>
		/// <exception cref="KeyNotFoundException"/>
		/// <returns>Computed value</returns>
		public double ComputeConstant() => expr.Compute(ImmutableDictionary<string, double>.Empty);

		public bool DependsOn(string variable)
			=> expr.Variables.Contains(variable);
	}
}


public abstract record Expr : IExpr
{
	public abstract IEnumerable<string> Variables { get; }
	public abstract int? PolynomialDegree { get; }
	public abstract double Compute(IReadOnlyDictionary<string, double> variables);


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
