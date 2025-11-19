namespace MathExpr.Operator;

public abstract record UnaryOperator(Expr Operand) : Expr
{
	// UnaryOperator:
	public abstract string Alias { get; }
	public abstract double ComputeFor(double value);

	// IExpr:
	public override IEnumerable<string> Variables => Operand.Variables;
	public sealed override int? PolynomialDegree => Operand.PolynomialDegree;
	public sealed override double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Operand.Compute(variables));

	// Object:
	public sealed override string ToString()
		=> $"{Alias}({Operand})";
}


public sealed record Negate(Expr Operand) : UnaryOperator(Operand)
{
	public override string Alias => "-";
	public override double ComputeFor(double value)
		=> -value;
}
