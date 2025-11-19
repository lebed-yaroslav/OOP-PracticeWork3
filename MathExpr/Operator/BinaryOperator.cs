namespace MathExpr.Operator;


public abstract record BinaryOperator(Expr Left, Expr Right) : Expr
{
	// BinaryOperator:
	public abstract string Alias { get; }
	public abstract double ComputeFor(double left, double right);

	// IExpr:
	public override IEnumerable<string> Variables =>
		Left.Variables.Union(Right.Variables);
	public override int? PolynomialDegree => (Left.IsPolynomial && Right.IsPolynomial) ?
		int.Max(Left.PolynomialDegree.Value, Right.PolynomialDegree.Value) : null;
	public sealed override double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Left.Compute(variables), Right.Compute(variables));

	// Object:
	public sealed override string ToString() => $"({Left} {Alias} {Right})";
}


public sealed record Add(Expr Left, Expr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "+";

	public override double ComputeFor(double left, double right)
		=> left + right;
}


public sealed record Subtract(Expr Left, Expr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "-";
	public override double ComputeFor(double left, double right)
		=> left - right;
}


public sealed record Multiply(Expr Left, Expr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "*";
	public override int? PolynomialDegree => (Left.IsPolynomial && Right.IsPolynomial) ?
		Left.PolynomialDegree.Value + Right.PolynomialDegree.Value : null;
	public override double ComputeFor(double left, double right)
		=> left * right;
}


public sealed record Divide(Expr Left, Expr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "/";
	public override int? PolynomialDegree => (Left.IsPolynomial && Right.IsConstant) ?
		Left.PolynomialDegree.Value : null;
	public override double ComputeFor(double left, double right)
		=> left / right;
}


public sealed record Power(Expr Left, Expr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "^";
	public override int? PolynomialDegree 
	{
		get
		{
			if (!Left.IsPolynomial || Right is not Constant c) // Only for simplified expressions
				return null;
			if (!double.IsInteger(c.Value))
				return null;
			int power = (int)c.Value;
			return power >= 0 ? Left.PolynomialDegree * power : null; 
		}
	}

	public override double ComputeFor(double left, double right)
		=> Math.Pow(left, right);
}
