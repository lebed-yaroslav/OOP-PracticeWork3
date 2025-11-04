using MathExpr.Util;

namespace MathExpr;

public sealed record Variable(string Name) : IExpr
{
	public IEnumerable<string> Variables => Name.Yield();
	public int? PolynomialDegree => 1;

	public double Compute(IReadOnlyDictionary<string, double> variables)
		=> variables[Name];

	public T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);

	public override string ToString() => Name;
}


public struct Constant(double value) : IExpr
{
	public double Value => value;

	public IEnumerable<string> Variables => Enumerable.Empty<string>();
	public int? PolynomialDegree => 0;

	public double Compute(IReadOnlyDictionary<string, double> variables)
		=> value;

	public T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);

	public override string ToString() => value.ToString();
}
