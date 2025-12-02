namespace MathExpr;

public sealed record Variable(string Name) : Expr
{
	public override IEnumerable<string> Variables => [Name];
	public override int? PolynomialDegree => 1;

	public override double Compute(IReadOnlyDictionary<string, double> variables)
		=> variables[Name];

	public override string ToString() => Name;
}


public sealed record Constant(double Value) : Expr
{
	public override IEnumerable<string> Variables => [];
	public override int? PolynomialDegree => 0;

	public override double Compute(IReadOnlyDictionary<string, double> variables)
		=> Value;

	public override string ToString() => 
		Value >= 0 ? $"{Value}" : $"({Value})";
}
