namespace MathExpr.Operator;


public abstract record BinaryOperator(IExpr Left, IExpr Right) : IExpr
{
	public abstract string Alias { get; }

	public IEnumerable<string> Variables =>
		Left.Variables.Concat(Right.Variables).Distinct();

	public virtual int? PolynomialDegree => (Left.IsPolynomial && Right.IsPolynomial) ?
		int.Max(Left.PolynomialDegree.Value, Right.PolynomialDegree.Value) : null;

	public double Compute(IReadOnlyDictionary<string, double> variables)
		=> ComputeFor(Left.Compute(variables), Right.Compute(variables));
	public abstract double ComputeFor(double left, double right);

	public abstract T Accept<T>(IExprVisitor<T> visitor);

	public sealed override string ToString() => $"({Left} {Alias} {Right})";
}


public sealed record Add(IExpr Left, IExpr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "+";
	public override double ComputeFor(double left, double right)
		=> left + right;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Subtract(IExpr Left, IExpr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "-";
	public override double ComputeFor(double left, double right)
		=> left - right;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Multiply(IExpr Left, IExpr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "*";
	public override int? PolynomialDegree => (Left.IsPolynomial && Right.IsPolynomial) ?
		Left.PolynomialDegree.Value + Right.PolynomialDegree.Value : null;
	public override double ComputeFor(double left, double right)
		=> left * right;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Divide(IExpr Left, IExpr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "/";
	public override int? PolynomialDegree => (Left.IsPolynomial && Right.IsConstant) ?
		Left.PolynomialDegree.Value : null;
	public override double ComputeFor(double left, double right)
		=> left / right;

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}


public sealed record Power(IExpr Left, IExpr Right) : BinaryOperator(Left, Right)
{
	public override string Alias => "^";
	public override int? PolynomialDegree 
	{
		get 
		{
			if (!Left.IsPolynomial || Right is not Constant c) // Only for simplifed expressions
				return null;
			if (!double.IsInteger(c.Value))
				return null;
			int power = (int)c.Value;
			return power >= 0 ? Left.PolynomialDegree * power : null; 
		}
	}

	public override double ComputeFor(double left, double right)
		=> Math.Pow(left, right);

	public override T Accept<T>(IExprVisitor<T> visitor) => visitor.Visit(this);
}
