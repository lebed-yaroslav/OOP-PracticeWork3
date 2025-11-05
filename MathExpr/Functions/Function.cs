namespace MathExpr.Functions;

public abstract record Function(IExpr Argument) : IExpr
{
	public string Name => GetType().Name; // Slow but simple

	public IEnumerable<string> Variables => Argument.Variables;
	public virtual int? PolynomialDegree => Argument.IsConstant ? 0 : null;

	public double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Argument.Compute(variables));

	public abstract double ComputeFor(double value);

	public abstract T Accept<T>(IExprVisitor<T> visitor);

	public sealed override string ToString() => $"{Name}({Argument})";
}
