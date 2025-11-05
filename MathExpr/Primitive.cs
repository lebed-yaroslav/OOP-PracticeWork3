using MathExpr.Util;

namespace MathExpr;

public sealed record Variable(string Name) : Expr
{
	public override IEnumerable<string> Variables => Name.Yield();
	public override int? PolynomialDegree => 1;

	public override double Compute(IReadOnlyDictionary<string, double> variables)
		=> variables[Name];

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);

	public override string ToString() => Name;
}


public sealed record Constant(double Value) : Expr
{
	public static readonly Constant Zero = new(0);
	public static readonly Constant One = new(1);

	public override IEnumerable<string> Variables => Enumerable.Empty<string>();
	public override int? PolynomialDegree => 0;

	public override double Compute(IReadOnlyDictionary<string, double> variables)
		=> Value;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);

	public override string ToString() => Value.ToString();
}
