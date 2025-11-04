namespace MathExpr.Operator;

public abstract record UnaryOperator(IExpr Operand) : IExpr
{
	public abstract string Alias { get; }

	public IEnumerable<string> Variables => Operand.Variables;
	public int? PolynomialDegree => Operand.PolynomialDegree;

	public double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Operand.Compute(variables));

	public abstract double ComputeFor(double value);

	public abstract T Accept<T>(IExprVisitor<T> visitor);

	public sealed override string ToString()
		=> $"{Alias}{Operand}";
}


public sealed record Negate(IExpr Operand) : UnaryOperator(Operand)
{
	public override string Alias => "-";
	public override double ComputeFor(double value)
		=> -value;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
