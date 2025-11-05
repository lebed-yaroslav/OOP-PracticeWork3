namespace MathExpr.Functions;

public abstract record Function(Expr Argument) : Expr
{
	// Function:
	public string Name => GetType().Name; // Slow but simple
	public abstract double ComputeFor(double value);

	// IExpr:
	public sealed override IEnumerable<string> Variables => Argument.Variables;
	public override int? PolynomialDegree => Argument.IsConstant ? 0 : null;
	public sealed override double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Argument.Compute(variables));

	// Object:
	public sealed override string ToString() => $"{Name}({Argument})";
}
